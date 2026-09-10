import { screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import type { CpuListItem } from '@/api/cpus.ts'

const listCpus = vi.fn()
const listManufacturersByProductType = vi.fn()
const listSockets = vi.fn()
const listCpuSeries = vi.fn()

vi.mock('@/api/cpus.ts', async () => {
  const actual = await vi.importActual<typeof import('@/api/cpus.ts')>('@/api/cpus.ts')
  return { ...actual, listCpus: (...args: unknown[]) => listCpus(...args) }
})

vi.mock('@/api/master-data.ts', async () => {
  const actual = await vi.importActual<typeof import('@/api/master-data.ts')>(
    '@/api/master-data.ts',
  )
  return {
    ...actual,
    listManufacturersByProductType: (...args: unknown[]) =>
      listManufacturersByProductType(...args),
    listSockets: () => listSockets(),
    listCpuSeries: () => listCpuSeries(),
  }
})

import { CpuListPage } from '@/pages/catalog/CpuListPage.tsx'
import { renderWithQuery } from '../helpers/query.tsx'

const cpu: CpuListItem = {
  id: 'cpu-1',
  name: 'Ryzen 7 7800X3D',
  manufacturerId: 'amd',
  manufacturerName: 'AMD',
  seriesId: 'r7',
  seriesName: 'Ryzen 7',
  socketId: 'am5',
  socketName: 'AM5',
  maxMemoryGb: 128,
  integratedGraphics: false,
  includedStockCooler: false,
  thermalDesignPower: 120,
  powerConsumptionWatts: 120,
}

describe('CpuListPage', () => {
  beforeEach(() => {
    listCpus.mockReset()
    listManufacturersByProductType.mockReset().mockResolvedValue([{ id: 'amd', name: 'AMD' }])
    listSockets.mockReset().mockResolvedValue([
      { id: 'am5', name: 'AM5', manufacturerId: 'amd', manufacturerName: 'AMD' },
    ])
    listCpuSeries.mockReset().mockResolvedValue([
      {
        id: 'r7',
        name: 'Ryzen 7',
        manufacturerId: 'amd',
        manufacturerName: 'AMD',
        socketId: 'am5',
        socketName: 'AM5',
      },
    ])
  })

  it('loads the unfiltered catalog', async () => {
    listCpus.mockResolvedValue({
      items: [cpu],
      totalCount: 1,
      pageIndex: 0,
      pageSize: 10,
    })
    renderWithQuery(<CpuListPage />, { route: '/catalog/cpus' })
    expect(await screen.findByRole('link', { name: 'Ryzen 7 7800X3D' })).toHaveAttribute(
      'href',
      '/catalog/cpus/cpu-1',
    )
    expect(listManufacturersByProductType).toHaveBeenCalledWith('cpu')
    expect(listCpus).toHaveBeenCalledWith(
      expect.objectContaining({ filter: expect.objectContaining({ name: '' }) }),
    )
    expect(screen.queryByLabelText('DDR')).not.toBeInTheDocument()
  })

  it('applies a name filter', async () => {
    listCpus.mockResolvedValue({
      items: [cpu],
      totalCount: 1,
      pageIndex: 0,
      pageSize: 10,
    })
    const user = userEvent.setup()
    renderWithQuery(<CpuListPage />, { route: '/catalog/cpus' })
    await screen.findByRole('link', { name: 'Ryzen 7 7800X3D' })
    await user.type(screen.getByLabelText('Name'), '7800')
    await user.click(screen.getByRole('button', { name: 'Apply filters' }))
    expect(await screen.findByRole('link', { name: 'Ryzen 7 7800X3D' })).toBeInTheDocument()
    expect(listCpus).toHaveBeenCalledWith(
      expect.objectContaining({ filter: expect.objectContaining({ name: '7800' }) }),
    )
  })

  it('shows an empty filtered state', async () => {
    listCpus.mockResolvedValue({ items: [], totalCount: 0, pageIndex: 0, pageSize: 10 })
    renderWithQuery(<CpuListPage />, { route: '/catalog/cpus?name=nope' })
    expect(await screen.findByText('No CPUs match these filters.')).toBeInTheDocument()
  })
})
