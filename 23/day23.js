var fs = require('fs')

var getValue = (ref, program) => {
  let val = parseInt(ref)

  if (isNaN(val)) {
    if (!(ref in program.registers)) {
      program.registers[ref] = 0
    }
    val = program.registers[ref]
  }

  return val
}

var runCommand = (program, data) => {
  let info = data.split(' ')
  let y
  let jumped = false

  switch (info[0]) {
    case 'set':
      program.registers[info[1]] = getValue(info[2], program)
      break
    case 'sub':
      y = getValue(info[2], program)

      if (!(info[1] in program.registers)) {
        info[1] = 0
      }

      program.registers[info[1]] -= y
      break
    case 'mul':
      y = getValue(info[2], program)

      if (!(info[1] in program.registers)) {
        info[1] = 0
      }

      program.registers[info[1]] *= y
      break
    case 'jnz':
      let j = getValue(info[1], program)
      if (j !== 0) {
        y = getValue(info[2], program)

        program.position += y
        jumped = true
      }
      break
  }

  program.position += jumped ? 0 : 1
}

fs.readFile('23/instructions.txt', (err, data) => {
  if (err) {
    console.error(err)
    process.exit()
  }

  let instructions = data.toString().split('\n')
  let program = {
    id: 0,
    registers: { a: 1 },
    position: 0
  }

  // while (program.position >= 0 && program.position < instructions.length) {
  //   runCommand(program, instructions[program.position])

  //   console.log('Got position ' + program.position)
  // }

  // console.log('Val in h ' + program.registers.h)
})

let b = 105700
let c = 122700
let d = 0
let e = 0
let f = 0
let g = 0
let h = 0

do {
  // Runs 1000 times
  f = 1
  d = 2
  do {
    e = 2
    for (let n = 0; n < (b - e); n++) {
      // Runs 105698 times
      if ((g - d) * e === b) {
        f = 0
      }
      e++
      g = e - b
    }
    g = 0
    d--
    g = d - b
  } while (g !== 0)
  if (f === 0) {
    h++
  }
  g = b - c
  if (g === 0) {
    b += 17
  } else {
    console.log('Got h ' + h)
    process.exit()
  }
} while (true)
