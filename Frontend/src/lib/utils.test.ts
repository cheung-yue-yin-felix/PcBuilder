import { describe, expect, it } from 'vitest'
import { cn } from './utils.ts'
import { buttonVariants } from '../components/ui/button-variants.ts'

describe('cn and buttonVariants', () => {
  it('merges class names', () => {
    expect(cn('a', false && 'b', 'c')).toContain('a')
    expect(cn('a', false && 'b', 'c')).toContain('c')
  })

  it('builds button classes', () => {
    expect(buttonVariants({ variant: 'outline', size: 'lg' })).toContain('border-border')
  })
})
