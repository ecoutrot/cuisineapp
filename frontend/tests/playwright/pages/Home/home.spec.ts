import { test, expect } from '@playwright/test';

test('home', async ({ page }) => {
  await page.goto('http://localhost:5173/');
  await page.getByRole('link', { name: 'Livre de cuisine' }).click();
  await expect(page.getByRole('navigation')).toContainText('Livre de cuisine');
});