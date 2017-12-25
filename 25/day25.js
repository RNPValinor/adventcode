var _ = require('lodash')

let tape = {}
let cursor = 0
let state = 'A'

var getValue = (cursor, tape) => {
  if (tape[cursor] === undefined) {
    tape[cursor] = 0
  }

  return tape[cursor]
}

for (let i = 0; i < 12302209; i++) {
  let curVal = getValue(cursor, tape)

  switch (state) {
    case 'A':
      tape[cursor] = (curVal + 1) % 2

      if (curVal === 0) {
        cursor++
        state = 'B'
      } else {
        cursor--
        state = 'D'
      }
      break
    case 'B':
      tape[cursor] = (curVal + 1) % 2
      cursor++

      if (curVal === 0) {
        state = 'C'
      } else {
        state = 'F'
      }
      break
    case 'C':
      tape[cursor] = 1
      cursor--

      if (curVal === 1) {
        state = 'A'
      }
      break
    case 'D':
      if (curVal === 0) {
        cursor--
        state = 'E'
      } else {
        cursor++
        state = 'A'
      }
      break
    case 'E':
      tape[cursor] = (curVal + 1) % 2

      if (curVal === 0) {
        cursor--
        state = 'A'
      } else {
        cursor++
        state = 'B'
      }
      break
    case 'F':
      tape[cursor] = 0
      cursor++

      if (curVal === 0) {
        state = 'C'
      } else {
        state = 'E'
      }
      break
  }
}

let counts = _.countBy(tape, (e) => { return e })

console.log('Num 1s: ' + counts[1])
