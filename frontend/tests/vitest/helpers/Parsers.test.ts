import { expect, test } from 'vitest'
import { toFloatOrNull, toIntOrNull, toStringOrNull } from '../../../src/helpers/Parsers'

test('toStringOrNull', () => {
    expect(toStringOrNull('')).toBeNull()
    expect(toStringOrNull(null)).toBeNull()
    expect(toStringOrNull('hello')).toBe('hello')
});

test('toIntOrNull', () => {
    expect(toIntOrNull('')).toBeNull()
    expect(toIntOrNull(null)).toBeNull()
    expect(toIntOrNull('10')).toBe(10)
    expect(toIntOrNull('10.5')).toBe(10)
    expect(toIntOrNull('hello')).toBeNull()
});

test('toFloatOrNull', () => {
    expect(toFloatOrNull('')).toBeNull()
    expect(toFloatOrNull(null)).toBeNull()
    expect(toFloatOrNull('10')).toBe(10)
    expect(toFloatOrNull('10.5')).toBe(10.5)
    expect(toFloatOrNull('hello')).toBeNull()
});