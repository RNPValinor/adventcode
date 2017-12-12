var _ = require('lodash')

let input = [197, 97, 204, 108, 1, 29, 5, 71, 0, 50, 2, 255, 248, 78, 254, 63]
let data = _.range(0, 256)
let curPos = 0

for (let skip = 0; skip < input.length; skip++) {
  let revLength = input[skip]
  let newData = _.clone(data)

  for (let j = 0; j < revLength; j++) {
    let index = (curPos + j) % data.length
    let revIndex = (curPos + revLength - 1 - j) % data.length
    newData[index] = data[revIndex]
  }

  data = newData

  curPos += revLength + skip
}

console.log('Unique values in data: ' + _.uniq(data).length)
console.log('Result: ' + (data[0] * data[1]))

let inputString = input.join(',')
input = _.map(inputString, (c) => { return c.charCodeAt(0) })
input = input.concat([17, 31, 73, 47, 23])

let numRounds = 64
data = _.range(0, 256)
curPos = 0
let skip = 0

while (numRounds > 0) {
  for (let i = 0; i < input.length; i++) {
    let revLength = input[i]
    let newData = _.clone(data)

    for (let j = 0; j < revLength; j++) {
      let index = (curPos + j) % data.length
      let revIndex = (curPos + revLength - 1 - j) % data.length
      newData[index] = data[revIndex]
    }

    data = newData

    curPos += revLength + skip
    skip++
  }

  numRounds--
}

console.log('Unique values in data after 64 rounds: ' + _.uniq(data).length)

let chunks = _.chunk(data, 16)
let denseHash = _.map(chunks, (c) => { return _.reduceRight(c, (acc, val) => { return acc ^ val }) })

let hexHash = _.map(denseHash, (n) => { return ('0' + n.toString(16)).substr(-2) })
let hexString = hexHash.join('')

console.log('Densifying result: ' + hexString + ', has length ' + hexString.length)
