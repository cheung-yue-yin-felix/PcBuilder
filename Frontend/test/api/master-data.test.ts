import { beforeEach, describe, expect, it, vi } from 'vitest'

const get = vi.fn()

vi.mock('@/api/client.ts', () => ({
  api: {
    get: (...args: unknown[]) => get(...args),
  },
}))

import { listManufacturersByProductType } from '@/api/master-data.ts'

describe('master-data API', () => {
  beforeEach(() => {
    get.mockReset()
  })

  it('lists manufacturers by product type', async () => {
    get.mockResolvedValue({ data: [{ id: 'amd', name: 'AMD' }] })
    await expect(listManufacturersByProductType('cpu')).resolves.toEqual([
      { id: 'amd', name: 'AMD' },
    ])
    expect(get).toHaveBeenCalledWith('/master-data/manufacturer/cpu')
  })
})
