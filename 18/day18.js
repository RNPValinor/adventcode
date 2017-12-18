var fs = require('fs')

var programs = [
  {
    id: 0,
    registers: { p: 0 },
    incoming: [],
    terminated: false,
    position: 0,
    waiting: false
  },
  {
    id: 1,
    registers: { p: 1 },
    incoming: [],
    terminated: false,
    position: 0,
    waiting: false
  }
]

var getValue = (ref, program) => {
  let val = parseInt(ref)

  if (isNaN(val)) {
    val = program.registers[ref]
  }

  return val
}

var runCommand = (program, data) => {
  let info = data.split(' ')
  let n
  let sent = false
  let jumped = false

  switch (info[0]) {
    case 'set':
      program.registers[info[1]] = getValue(info[2], program)
      break
    case 'add':
      n = getValue(info[2], program)

      if (!(info[1] in program.registers)) {
        info[1] = 0
      }

      program.registers[info[1]] += n
      break
    case 'mul':
      n = getValue(info[2], program)

      if (!(info[1] in program.registers)) {
        info[1] = 0
      }

      program.registers[info[1]] *= n
      break
    case 'mod':
      n = getValue(info[2], program)

      if (!(info[1] in program.registers)) {
        info[1] = 0
      }

      program.registers[info[1]] = program.registers[info[1]] % n
      break
    case 'rcv':
      if (program.incoming.length > 0) {
        program.registers[info[1]] = program.incoming.shift()
      } else {
        program.waiting = true
        program.waitingRegister = info[1]
      }
      break
    case 'jgz':
      let j = getValue(info[1], program)
      if (j > 0) {
        n = getValue(info[2], program)

        program.position += n
        jumped = true
      }
      break
    case 'snd':
      n = getValue(info[1], program)
      programs[(program.id + 1) % 2].incoming.push(n)
      sent = true
      break
  }

  program.position += jumped ? 0 : 1

  return sent ? 1 : 0
}

fs.readFile('instructions.txt', (err, data) => {
  let lines = data.toString().split('\r\n')
  let numSent = 0
  let p0 = programs[0]
  let p1 = programs[1]

  while (!(p0.waiting && p1.waiting) && !(p0.terminated && p1.terminated) && !(p0.terminated && p1.waiting) && !(p1.terminated && p0.waiting)) {
    if (p0.waiting) {
      if (p0.incoming.length) {
        p0.registers[p0.waitingRegister] = p0.incoming.shift()
        p0.waiting = false
      }
    } else {
      runCommand(p0, lines[p0.position])
      if (p0.position < 0 || p0.positon >= lines.length) {
        p0.terminated = true
      }
    }

    if (p1.waiting) {
      if (p1.incoming.length) {
        p1.registers[p1.waitingRegister] = p1.incoming.shift()
        p1.waiting = false
      }
    } else {
      numSent += runCommand(p1, lines[p1.position])
      if (p1.position < 0 || p1.positon >= lines.length) {
        p1.terminated = true
      }
      console.log('Num sent by P1: ' + numSent)
    }

    if (p0.terminated || p0.waiting) {
      console.log('P0 status: pos ' + p0.position + ', ' + (p0.terminated ? 'terminated' : p0.waiting ? 'waiting' : ''))
    }
    if (p1.terminated || p1.waiting) {
      console.log('P1 status: pos ' + p1.position + ', ' + (p1.terminated ? 'terminated' : p1.waiting ? 'waiting' : ''))
    }
  }

  console.log('Num sent by p1: ' + numSent)
})
