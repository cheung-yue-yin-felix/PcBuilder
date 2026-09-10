import { keepPreviousData, useQuery } from '@tanstack/react-query'
import { cpuKeys, getCpuById, listCpus, type CpuListParams } from '@/api/cpus.ts'

export function useCpus(params: CpuListParams) {
  return useQuery({
    queryKey: cpuKeys.list(params),
    queryFn: () => listCpus(params),
    placeholderData: keepPreviousData,
  })
}

export function useCpu(id: string | undefined) {
  return useQuery({
    queryKey: cpuKeys.detail(id ?? ''),
    queryFn: () => getCpuById(id!),
    enabled: Boolean(id),
  })
}
