import { describe, expect, it } from 'vitest'
import {
  cpuListParamsFromSearch,
  cpuListSearchFromParams,
  emptyCpuFilter,
} from '@/api/cpu-list-params.ts'

describe('cpu list search params', () => {
  it('uses GET-friendly empty filter by default', () => {
    expect(cpuListParamsFromSearch(new URLSearchParams())).toEqual({
      pageIndex: 0,
      pageSize: 10,
      sortBy: 'name',
      sortDirection: 'asc',
      filter: {
        name: '',
        manufacturerId: undefined,
        socketId: undefined,
        seriesId: undefined,
        thermalDesignPower: undefined,
        powerConsumptionWatts: undefined,
      },
    })
  })

  it('round-trips name, paging, and range filters without ddrGeneration', () => {
    const params = {
      pageIndex: 2,
      pageSize: 10,
      sortBy: 'name' as const,
      sortDirection: 'asc' as const,
      filter: {
        name: 'Ryzen',
        manufacturerId: 'amd',
        thermalDesignPower: { min: 65, max: 170 },
      },
    }
    const search = cpuListSearchFromParams(params)
    expect(search.get('ddrGeneration')).toBeNull()
    expect(cpuListParamsFromSearch(search)).toMatchObject({
      pageIndex: 2,
      filter: {
        name: 'Ryzen',
        manufacturerId: 'amd',
        thermalDesignPower: { min: 65, max: 170 },
      },
    })
  })

  it('omits empty filter from the query string', () => {
    expect(cpuListSearchFromParams({
      pageIndex: 0,
      pageSize: 10,
      filter: emptyCpuFilter,
    }).toString()).toBe('')
  })
})
