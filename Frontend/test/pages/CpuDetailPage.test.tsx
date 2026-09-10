import { screen } from '@testing-library/react'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import type { CpuDetail } from '@/api/cpus.ts'

const getCpuById = vi.fn()

vi.mock('@/api/cpus.ts', async () => {
  const actual = await vi.importActual<typeof import('@/api/cpus.ts')>('@/api/cpus.ts')
  return { ...actual, getCpuById: (...args: unknown[]) => getCpuById(...args) }
})

import { CpuDetailPage } from '@/pages/catalog/CpuDetailPage.tsx'
import { renderWithQuery } from '../helpers/query.tsx'
import { Route, Routes } from 'react-router-dom'

const cpu: CpuDetail = {
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
  ramCompats: [
    { ddrGeneration: 'Ddr5', ramModuleCount: 2, ramRank: 'DualRank', maxSpeedMts: 6000 },
  ],
  supportChipsets: [{ chipsetId: 'x670', chipsetName: 'X670', requiresBiosUpdate: false }],
}

function renderDetail(route = '/catalog/cpus/cpu-1') {
  return renderWithQuery(
    <Routes>
      <Route path="/catalog/cpus/:cpuId" element={<CpuDetailPage />} />
    </Routes>,
    { route },
  )
}

describe('CpuDetailPage', () => {
  beforeEach(() => {
    getCpuById.mockReset()
  })

  it('renders CPU details and RAM compatibility', async () => {
    getCpuById.mockResolvedValue(cpu)
    renderDetail()
    expect(await screen.findByRole('heading', { name: 'Ryzen 7 7800X3D' })).toBeInTheDocument()
    expect(screen.getByText('AMD')).toBeInTheDocument()
    expect(screen.getByText('DDR5')).toBeInTheDocument()
    expect(screen.getByText('X670')).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Back to CPUs' })).toHaveAttribute(
      'href',
      '/catalog/cpus',
    )
  })

  it('shows an API error', async () => {
    getCpuById.mockRejectedValue(new Error('fail'))
    renderDetail()
    expect(await screen.findByText('Something went wrong. Try again.')).toBeInTheDocument()
  })
})
