import { Page } from '@playwright/test';

type State = {
    selectedType: string | null,
    selectedPhase: string | null,
    registeredUser: any | null,
    newUser: any | null,
    votestate: any | null,
}

export const state: State = {
    selectedType: null,
    selectedPhase: null,
    registeredUser: null,
    newUser: null,
    votestate: null,
}

export function resetState() {
    state.selectedType = null;
    state.selectedPhase = null;
    state.votestate = null;
}

export function hardResetState() {
    state.selectedType = null;
    state.selectedPhase = null;
    state.votestate = null;
    state.registeredUser = null;
    state.newUser = null;
}
export async function setupUser(p: Page) {
    const id = Date.now();
    await p.goto('/register');
    await p.getByRole('textbox', { name: 'e-mail cím' }).fill(`tester${id}@test.com`);
    await p.getByRole('textbox', { name: 'felhasználónév' }).fill(`tester${id}`);
    await p.getByRole('textbox', { name: 'jelszó' }).fill(`Asd@123.com`);
    await p.locator('form').getByRole('button', { name: 'Regisztráció' }).click();

    const [response] = await Promise.all([
        p.waitForResponse((r) => r.url().includes('/api/user/register') && r.request().method() === 'POST'),
        p.locator('form').getByRole('button', { name: 'Regisztráció' }).click(),
    ]);

    state.registeredUser = { name: `tester${id}`, email: `tester${id}@test.com`, password: `Asd@123.com` };
    await p.goto('home');
}

export function makeUser() {
    const id = Date.now();
    return { name: `tester${id}`, email: `tester${id}@test.com`, password: `Asd@123.com` };
}