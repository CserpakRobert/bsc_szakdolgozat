import { Page } from '@playwright/test';

type State = {
    selectedType : string | null,
    selectedPhase : string | null,
    registeredUser : any | null,
    newUser : any | null,
}

export const state: State = {
    selectedType: null,
    selectedPhase: null,
    registeredUser: null,
    newUser: null,
}

export function resetState() {
    state.selectedType = null;
    state.selectedPhase = null;
}

export function hardResetState() {
    state.selectedType = null;
    state.selectedPhase = null;
    state.registeredUser = null;
    state.newUser = null;
}
export async function setupUser(p:Page){
        const id = Date.now();
        await p.goto('/register');
        await p.getByRole('textbox', { name: 'e-mail cím' }).fill(`tester${id}@test.com`);
        await p.getByRole('textbox', { name: 'felhasználónév' }).fill(`tester${id}@test.com`);
        await p.getByRole('textbox', { name: 'jelszó' }).fill(`tester${id}@test.com`);
        await p.locator('form').getByRole('button', { name: 'Regisztráció' }).click();
        await p.getByRole('button', { name: 'Kijelentkezés' }).click();
        state.registeredUser = { name: `tester${id}@test.com`, email: `tester${id}@test.com`, password: `tester${id}@test.com` };
}

export function makeUser() {
  const id = Date.now();
  return { name: `tester${id}@test.com`, email: `tester${id}@test.com`, password: `tester${id}@test.com` };
}