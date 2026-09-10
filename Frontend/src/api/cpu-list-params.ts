import type { CpuFilter, CpuListParams } from '@/api/cpus.ts'
import { hasCompleteRange } from '@/api/paging.ts'

const PAGE_SIZE = 10

export const emptyCpuFilter: CpuFilter = {
  name: '',
}

export function cpuListParamsFromSearch(search: URLSearchParams): CpuListParams {
  return {
    pageIndex: Math.max(0, toInteger(search.get('page')) ?? 0),
    pageSize: PAGE_SIZE,
    sortBy: 'name',
    sortDirection: 'asc',
    filter: filterFromSearch(search),
  }
}

export function cpuListSearchFromParams(params: CpuListParams): URLSearchParams {
  const search = new URLSearchParams()
  const filter = params.filter

  if (params.pageIndex > 0) search.set('page', String(params.pageIndex))
  if (filter.name?.trim()) search.set('name', filter.name.trim())
  if (filter.manufacturerId) search.set('manufacturerId', filter.manufacturerId)
  if (filter.socketId) search.set('socketId', filter.socketId)
  if (filter.seriesId) search.set('seriesId', filter.seriesId)
  if (hasCompleteRange(filter.thermalDesignPower)) {
    search.set('tdpMin', String(filter.thermalDesignPower!.min))
    search.set('tdpMax', String(filter.thermalDesignPower!.max))
  }
  if (hasCompleteRange(filter.powerConsumptionWatts)) {
    search.set('powerMin', String(filter.powerConsumptionWatts!.min))
    search.set('powerMax', String(filter.powerConsumptionWatts!.max))
  }

  return search
}

function filterFromSearch(search: URLSearchParams): CpuFilter {
  return {
    name: search.get('name') ?? '',
    manufacturerId: emptyToUndefined(search.get('manufacturerId')),
    socketId: emptyToUndefined(search.get('socketId')),
    seriesId: emptyToUndefined(search.get('seriesId')),
    thermalDesignPower: parseRange(search.get('tdpMin'), search.get('tdpMax')),
    powerConsumptionWatts: parseRange(search.get('powerMin'), search.get('powerMax')),
  }
}

function parseRange(minValue: string | null, maxValue: string | null) {
  const min = toInteger(minValue)
  const max = toInteger(maxValue)
  if (min == null || max == null) return undefined
  return { min, max }
}

function toInteger(value: string | null): number | undefined {
  if (value == null || value.trim() === '') return undefined
  const parsed = Number(value)
  return Number.isInteger(parsed) ? parsed : undefined
}

function emptyToUndefined(value: string | null): string | undefined {
  return value?.trim() ? value : undefined
}
