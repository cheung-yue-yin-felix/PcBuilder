export type RangeFilter = {
  min?: number | null
  max?: number | null
}

export type PagedRequest = {
  pageIndex: number
  pageSize: number
  sortBy?: string
  sortDirection?: 'asc' | 'desc'
}

export type PagedResult<T> = {
  pageIndex: number
  pageSize: number
  totalCount: number
  items: T[]
}

export function hasCompleteRange(range?: RangeFilter | null): boolean {
  return range != null && range.min != null && range.max != null
}
