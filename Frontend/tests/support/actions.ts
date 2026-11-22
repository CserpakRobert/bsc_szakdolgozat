import { Page } from '@playwright/test';

const user = { email: "", password: "" };
const userFail = { email: "fail@fail.fail", password: "fail" };
export const actions: Record<string, (p: Page) => Promise<void>> = {
    navigateMain: async (p) => await p.getByRole('button', { name: 'Főoldal' }).click(),
    navigateLogin: async (p) => await p.getByRole('link', { name: 'Bejelentkezés' }).getByRole('button').click(),
    login: async (p) => {
        await p.getByRole('textbox', { name: 'e-mail cím' }).fill(user.email);
        await p.getByRole('textbox', { name: 'jelszó' }).fill(user.password);
        await p.locator('form').getByRole('button', { name: 'Bejelentkezés' }).click();
    },
    loginFailed: async (p) => {
        await p.getByRole('textbox', { name: 'e-mail cím' }).fill(userFail.email);
        await p.getByRole('textbox', { name: 'jelszó' }).fill(userFail.password);
        await p.locator('form').getByRole('button', { name: 'Bejelentkezés' }).click();
    },
    navigateRegister: async (p) => await p.getByRole('link', { name: 'Regisztráció' }).getByRole('button').click(),
    registerSuccess: async (p) => { return },
    registerFailed: async (p) => { return },
    navigateCreate: async (p) => { return },
    createSuccess: async (p) => { return },
    createFailed: async (p) => { return },
    navigateLists: async (p) => { return },
    selectPhase: async (p) => { return },
    selectType: async (p) => { return },
    paginateList: async (p) => { return },
    selectPhase1: async (p) => { return },
    navigateCompetitionPhase0: async (p) => { return },
    navigateCompetition: async (p) => { return },
    paginate: async (p) => { return },
    navigateSubmission: async (p) => { return },
    vote: async (p) => { return },
    logout: async (p) => { return }


};