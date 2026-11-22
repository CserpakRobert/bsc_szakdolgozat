import { Page, expect } from '@playwright/test';
import { state, resetState } from './data';

export const checks: Record<string, (p: Page) => Promise<void>> = {

    navigateMain: async p => await expect(p).toHaveURL('/home'),
    navigateLogin: async p => await expect(p).toHaveURL('/login'),
    loginSuccess: async p => await expect(p).toHaveURL('/home'),
    loginFailed: async p => await expect(p.getByRole('alert')).toBeVisible(),
    navigateRegister: async p => await expect(p).toHaveURL('/register'),
    registerSuccess: async p => await expect(p).toHaveURL('/login'),
    registerFailed: async p => await expect(p.getByRole('alert')).toBeVisible(),
    navigateCreate: async p => await expect(p).toHaveURL('/competition/create'),
    createSuccess: async p => await expect(p).toHaveURL(/\/competition\/details\/\d+$/),
    createFailed: async p => { await expect(p.getByRole('alert')).toBeVisible() },
    navigateLists: async p => expect(p).toHaveURL('/competition/list'),
    selectPhase: async p => {
        await phaseCheck(p);
        resetState();
    },
    selectType: async p => {
        await typeCheck(p);
        resetState();
    },
    paginateList: async p => {
        const next = p.getByRole('button', { name: 'Go to next page' }); 
        await expect
            .poll(async () => (await next.isHidden()) || (await next.isDisabled()))
            .toBe(true);
    },
    selectPhase1: async p => { expect(p).toHaveURL(/\/competition\/details\/\d+$/) },
    navigateCompetitionPhase0: async p => { expect(p).toHaveURL(/\/competition\/details\/\d+$/) },
    navigateCompetition: async p => { expect(p).toHaveURL(/\/competition\/details\/\d+$/) },
    paginate: async p => {
        const next = p.getByRole('button', { name: 'Go to next page' }); 
        await expect
            .poll(async () => (await next.isHidden()) || (await next.isDisabled()))
            .toBe(true);
    },
    navigateSubmission: async p => { await expect(p).toHaveURL(/\/submission\/\d+$/) },
    vote: async p => {
        if (state.votestate === 'voted') {
            await expect(p.getByRole('button', { name: 'visszavonás' })).toBeVisible();
        } else if (state.votestate === 'unvoted') {
            await expect(p.getByRole('button', { name: 'Szavazás' })).toBeVisible();
        }
    },
    logout: async p => {
        await expect(p).toHaveURL('/login');

    },


};

const typeLabels: Record<string, string> = {
  Film: 'rövidfilm',
  Zene: 'zene',
  Történet: 'történet',
};

async function typeCheck(p: Page) {
  if (!state.selectedType) return; 

  for (const [type, text] of Object.entries(typeLabels)) {
    if (type !== state.selectedType) {
      await expect(p.getByText(text)).toHaveCount(0);
    }
  }
}

const phases = ['Jelentkezés', /*'Beadás',*/ 'Szavazás', 'Befejezett'];

async function phaseCheck(p: Page) {
  if (!state.selectedPhase) return;  

  for (const phase of phases) {
    if (phase !== state.selectedPhase) {
      await expect(p.getByText(phase, { exact: true })).toHaveCount(0);
    }
  }
}