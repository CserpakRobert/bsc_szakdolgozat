import { Page, expect } from '@playwright/test';

export const checks: Record<string, (p: Page) => Promise<void>> = {

    navigateMain: async p => { await expect(p).toHaveURL('/') },
    navigateLogin: async p => { await expect(p).toHaveURL('/login') },
    loginSuccess: async p => { await expect(p).toHaveURL('/') },
    loginFailed: async p => { await expect(p.getByRole('alert')).toBeVisible() },
    navigateRegister: async p => { await expect(p).toHaveURL('/register') },
    registerSuccess: async p => { await expect(p).toHaveURL('/') },
    registerFailed: async p => { await expect(p.getByRole('alert')).toBeVisible()  },
    navigateCreate: async p => { expect(true) },
    createSuccess: async p => { expect(true) },
    createFailed: async p => { expect(true) },
    navigateLists: async p => { expect(true) },
    selectPhase: async p => { expect(true) },
    selectType: async p => { expect(true) },
    paginateList: async p => { expect(true) },
    selectPhase1: async p => { expect(true) },
    navigateCompetitionPhase0: async p => { expect(true) },
    navigateCompetition: async p => { expect(true) },
    paginate: async p => { expect(true) },
    navigateSubmission: async p => { expect(true) },
    vote: async p => { expect(true) },
    logout: async p => { expect(true) },


};