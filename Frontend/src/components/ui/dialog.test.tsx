import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { describe, expect, it } from 'vitest'
import { Button } from './button.tsx'
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from './dialog.tsx'
import { Input } from './input.tsx'
import { Label } from './label.tsx'

describe('shadcn primitives', () => {
  it('opens a dialog with header, description, and footer', async () => {
    const user = userEvent.setup()
    render(
      <Dialog>
        <DialogTrigger asChild>
          <Button>Open</Button>
        </DialogTrigger>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Title</DialogTitle>
            <DialogDescription>Details</DialogDescription>
          </DialogHeader>
          <DialogFooter showCloseButton>
            <DialogClose asChild>
              <Button type="button">Done</Button>
            </DialogClose>
          </DialogFooter>
        </DialogContent>
      </Dialog>,
    )
    await user.click(screen.getByRole('button', { name: 'Open' }))
    expect(await screen.findByText('Title')).toBeInTheDocument()
    expect(screen.getByText('Details')).toBeInTheDocument()
  })

  it('renders input and label', () => {
    render(
      <>
        <Label htmlFor="n">Name</Label>
        <Input id="n" />
      </>,
    )
    expect(screen.getByLabelText('Name')).toBeInTheDocument()
  })
})
