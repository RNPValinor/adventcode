const input = 345
var buffer = [0]
var index = 0

for (let i = 1; i <= 2017; i++) {
  index = (input + index) % buffer.length
  index++

  if (index > buffer.length) {
    buffer.push(i)
  } else {
    buffer.splice(index, 0, i)
  }
}

let targetIndex = (index + 1) % buffer.length

console.log('Target number is ' + buffer[targetIndex])

let lastAdded = 0
index = 0

for (let i = 1; i < 50000000; i++) {
  index = (input + index) % i
  index++

  if (index === 1) {
    lastAdded = i
  }

  if (i % 500000 === 0) {
    console.log((i / 500000) + '% complete')
  }
}

console.log('Number after 0 is ' + lastAdded)
