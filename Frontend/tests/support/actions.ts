import { Page } from '@playwright/test';
import { state, resetState, makeUser } from './data';

;
const userFail = { email: "fail@fail.fail", password: "fail" };
export const actions: Record<string, (p: Page) => Promise<void>> = {
    navigateMain: async (p) => await p.getByRole('button', { name: 'Főoldal' }).click(),
    navigateLogin: async (p) => await p.getByRole('link', { name: 'Bejelentkezés' }).getByRole('button').click(),
    login: async (p) => {
        const user = state.newUser ?? state.registeredUser;
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
    registerSuccess: async (p) => {
        resetState();
        const user = makeUser();
        state.newUser = user;
        await p.getByRole('textbox', { name: 'e-mail cím' }).fill(user.email);
        await p.getByRole('textbox', { name: 'felhasználónév' }).fill(user.name);
        await p.getByRole('textbox', { name: 'jelszó' }).fill(user.password);
        await p.locator('form').getByRole('button', { name: 'Regisztráció' }).click();
    },
    registerFailed: async (p) => {
        await p.getByRole('textbox', { name: 'e-mail cím' }).fill(userFail.email);
        await p.getByRole('textbox', { name: 'felhasználónév' }).fill(userFail.email);
        await p.getByRole('textbox', { name: 'jelszó' }).fill(userFail.password);
        await p.locator('form').getByRole('button', { name: 'Regisztráció' }).click();
    },
    navigateCreate: async (p) => await p.getByRole('link', { name: 'Verseny készítése' }).getByRole('button').click(),
    createSuccess: async (p) => {
        await p.getByRole('textbox', { name: 'Verseny Címe' }).fill('Test Verseny');
        await p.getByRole('textbox', { name: 'Verseny leírása' }).fill('Ez egy teszt verseny leírása');

        const types = ['Film', 'Zene', 'Történet'];
        const type = types[Math.floor(Math.random() * types.length)];
        await p.getByRole('radio', { name: type }).check();
        await p.getByRole('button', { name: 'Verseny Készítése', exact: true }).click();
    },
    createFailed: async (p) => {
        const types = ['Film', 'Zene', 'Történet'];
        const type = types[Math.floor(Math.random() * types.length)];
        await p.getByRole('textbox', { name: 'Verseny Címe' }).fill('');
        await p.getByRole('textbox', { name: 'Verseny leírása' }).fill('Ez egy teszt verseny leírása');
        await p.getByRole('radio', { name: type }).check();
        await p.getByRole('button', { name: 'Verseny Készítése', exact: true }).click();
    },
    navigateLists: async (p) => {
        state.selectedPhase = null;
        state.selectedType = null;
        await p.getByRole('button', { name: 'Verseynek Listája' }).click()
    },
    selectPhase: async (p) => {
        await p.goto('/competition/list');
        state.selectedPhase = null;
        state.selectedType = null;
        const phases = ['Jelentkezés', 'Beadás', 'Szavazás', 'Befejezett'];
        const phase = phases[Math.floor(Math.random() * phases.length)];
        state.selectedPhase = phase;
        await p.getByText('Minden Fázis').click();
        await p.getByRole('option', { name: phase }).click();
    },
    selectType: async (p) => {
        await p.goto('/competition/list');
        state.selectedPhase = null;
        state.selectedType = null;
        const types = ['Film', 'Zene', 'Történet'];
        const type = types[Math.floor(Math.random() * types.length)];
        state.selectedType = type;
        await p.getByText('Minden Típus').click();
        await p.getByRole('option', { name: type }).click();
    },
    paginateList: async (p) => {
        await p.goto('/competition/list');
        state.selectedPhase = null;
        state.selectedType = null;
        while (await p.getByRole('button', { name: 'Go to next page' }).isVisible() &&await p.getByRole('button', { name: 'Go to next page' }).isEnabled()) {
            await p.getByRole('button', { name: 'Go to next page' }).click();
        }
    },
    selectPhase1: async (p) => { await p.goto('/competition/details/2') },
    navigateCompetitionPhase0: async (p) => { await p.goto('/competition/details/1') },
    navigateCompetition: async (p) => {
        await p.goto('/competition/details/2')
    },
    paginate: async (p) => {
        while (await p.getByRole('button', { name: 'Go to next page' }).isVisible() &&await p.getByRole('button', { name: 'Go to next page' }).isEnabled()) {
            await p.getByRole('button', { name: 'Go to next page' }).click();
        }
    },
    navigateSubmission: async (p) => await p.getByRole('button', { name: 'Pályázat megtekintése' }).click(),
    vote: async (p) => {
        if (await p.getByRole('button', { name: 'Szavazás' }).isVisible()) {
            await p.getByRole('button', { name: 'Szavazás' }).click();
            state.votestate = 'voted';
        } else if (await p.getByRole('button', { name: 'visszavonás' }).isVisible()) {
            await p.getByRole('button', { name: 'visszavonás' }).click();
            state.votestate = 'unvoted';
        }else state.votestate = null;
    },
    logout: async (p) => await p.getByRole('button', { name: 'Kijelentkezés' }).click(),


};