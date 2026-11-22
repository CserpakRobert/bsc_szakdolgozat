import { test } from '@playwright/test';
import fs from 'fs';
import path from 'path';
import { actions } from './support/actions';
import { checks } from './support/checks';

const suitesDir = path.join('C:\\Users\\Bobelt\\Desktop\\szakdolgozat2025\\Frontend\\tests\\suites');
const files = fs.readdirSync(suitesDir).filter((f) => f.endsWith('.json')).sort();



test.beforeEach(async ({ request }) => {
    //TODO
  console.log(`test`);
});

for (const file of files) {

    // Load suite
    const { name, method, input_list, output_list } =
        JSON.parse(fs.readFileSync(path.join(suitesDir, file), 'utf8')).test_suite;
    
    //TODO remove only for testing
    const max = Number(12);
    test(`${name} ${method}`, async ({ page }) => {
        await page.goto('/home');
        for (let i = 0; i < max; i++) {
            const inp = input_list[i];
            const out = output_list[i];
            await test.step(`${i}: ${inp} -> ${out}`, async () => {
                if (!actions[inp]) throw new Error(`Not implemented: input "${inp}"`);
                if (!checks[out]) throw new Error(`Not implemented: output "${out}"`);
                await actions[inp](page);
                await checks[out](page);
            });
        }
    });
}