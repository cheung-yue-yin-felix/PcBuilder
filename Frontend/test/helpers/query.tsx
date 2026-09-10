import { QueryClientProvider } from '@tanstack/react-query'
import { render, type RenderOptions } from '@testing-library/react'
import type { ReactElement } from 'react'
import { MemoryRouter } from 'react-router-dom'
import { createQueryClient } from '@/query/query-client.ts'

export function renderWithQuery(
  ui: ReactElement,
  options?: { route?: string } & Omit<RenderOptions, 'wrapper'>,
) {
  const { route = '/', ...renderOptions } = options ?? {}
  const queryClient = createQueryClient({ retry: false })

  return {
    queryClient,
    ...render(ui, {
      wrapper: ({ children }) => (
        <QueryClientProvider client={queryClient}>
          <MemoryRouter initialEntries={[route]}>{children}</MemoryRouter>
        </QueryClientProvider>
      ),
      ...renderOptions,
    }),
  }
}
