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
})

let b = 105700
let c = 122700
let h = 0

let primes = []
for (let i = 0; i < c; i++) {
  primes[i] = true
}

var limit = Math.sqrt(c)
for (var i = 2; i < limit; i++) {
  if (primes[i] === true) {
    for (var j = i * i; j < c; j += i) {
      primes[j] = false
    }
  }
}

for (b = 105700; b !== c; b += 17) {
  if (!primes[b]) {
    h++
  }
}

console.log('Got h ' + h)

// h > 914
