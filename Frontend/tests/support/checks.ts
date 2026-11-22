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
    navigateCreate: async p => await expect(p).toHaveURL('/create'),
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
        //TODO FILL TESTS
        expect(true) 
    },
    selectPhase1: async p => { expect(true) },
    navigateCompetitionPhase0: async p => { expect(true) },
    navigateCompetition: async p => { expect(true) },
    paginate: async p => { expect(true) },
    navigateSubmission: async p => { expect(true) },
    vote: async p => { expect(true) },
    logout: async p => {
        await expect(p).toHaveURL('/home');

    },


};

function typeCheck(p: Page) {
    return true;
}

function phaseCheck(p: Page) {
    return true;
}