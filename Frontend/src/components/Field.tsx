import type { HTMLInputTypeAttribute } from 'react'

type FieldProps = {
  id: string
  label: string
  type?: HTMLInputTypeAttribute
  value: string
  autoComplete?: string
  error?: string
  hint?: string
  required?: boolean
  onChange: (value: string) => void
}

export function Field({
  id,
  label,
  type = 'text',
  value,
  autoComplete,
  error,
  hint,
  required,
  onChange,
}: Readonly<FieldProps>) {
  const errorId = `${id}-error`
  const hintId = `${id}-hint`
  const noError = hint ? hintId : undefined

  return (
    <div className="auth-field">
      <label htmlFor={id}>{label}</label>
      <input
        id={id}
        name={id}
        type={type}
        value={value}
        autoComplete={autoComplete}
        required={required}
        aria-invalid={error ? true : undefined}
        aria-describedby={error ? errorId : noError}
        onChange={(event) => onChange(event.target.value)}
      />
      {hint && !error ? (
        <p id={hintId} className="field-hint">
          {hint}
        </p>
      ) : null}
      {error ? (
        <p id={errorId} className="field-error">
          {error}
        </p>
      ) : null}
    </div>
  )
}
