import { Page } from '@playwright/test';

type State = {
    selectedType : string | null,
    selectedPhase : string | null,
}

export const state: State = {
    selectedType: null,
    selectedPhase: null,
}

export function resetState() {
    state.selectedType = null;
    state.selectedPhase = null;
}

export async function setupUser(p:Page){
        const id = Date.now();
        await p.goto('/register');
        await p.getByRole('textbox', { name: 'e-mail cím' }).fill(`tester${id}`);
        await p.getByRole('textbox', { name: 'felhasználónév' }).fill(`tester${id}`);
        await p.getByRole('textbox', { name: 'jelszó' }).fill(`tester${id}`);
        await p.locator('form').getByRole('button', { name: 'Regisztráció' }).click();
        await p.getByRole('button', { name: 'Kijelentkezés' }).click();
}

export function makeUser() {
  const id = Date.now();
  return { name: `tester${id}`, email: `tester${id}@test.com`, password: `tester${id}@test.com` };
}