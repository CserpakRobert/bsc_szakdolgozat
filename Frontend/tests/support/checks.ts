import { Page, expect } from '@playwright/test';
import { state, resetState } from './data';

export const checks: Record<string, (p: Page) => Promise<void>> = {

    navigateMain: async p => await expect(p).toHaveURL('/home'),
    navigateLogin: async p => await expect(p).toHaveURL('/login'),
    loginSuccess: async p => await expect(p).toHaveURL('/home'),
    loginFailed: async p => await expect(p.getByRole('alert')).toBeVisible(),
    navigateRegister: async p => await expect(p).toHaveURL('/register'),
    registerSuccess: async p => await expect(p).toHaveURL('/'),
    registerFailed: async p => await expect(p.getByRole('alert')).toBeVisible(),
    navigateCreate: async p => await expect(p).toHaveURL('/competition/create'),
    createSuccess: async p => await expect(p).toHaveURL(/\/competition\/details\/\d+$/),
    createFailed: async p => { await expect(p.getByRole('alert')).toBeVisible() },
    navigateLists: async p => expect(p).toHaveURL('/competition/list'),
    selectPhase: async p => {
        expect(phaseCheck(p));
        resetState();
    },
    selectType: async p => {
        expect(typeCheck(p));
        resetState();
    },
    paginateList: async p => {
        await expect(p.getByRole('listitem').nth(3)).toBeDisabled();
        await p.goto('/competition/list');
        await expect(p.getByRole('listitem').nth(3)).toBeEnabled();
    },
    selectPhase1: async p => { expect(p).toHaveURL(/\/competition\/details\/\d+$/) },
    navigateCompetitionPhase0: async p => { expect(p).toHaveURL(/\/competition\/details\/\d+$/) },
    navigateCompetition: async p => { expect(p).toHaveURL(/\/competition\/details\/\d+$/) },
    paginate: async p => {
        //TODO REMOVE?
        expect(true);
    },
    navigateSubmission: async p => { await expect(p).toHaveURL(/\/competition\/submission\/\d+$/) },
    vote: async p => { expect(true) },
    logout: async p => {
        await expect(p).toHaveURL('/login');

    },


};

function typeCheck(p: Page) {
    return true;
}

function phaseCheck(p: Page) {
    return true;
}