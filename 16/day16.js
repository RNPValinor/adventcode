var fs = require('fs')
var ProgressBar = require('progress')

fs.readFile('moves.txt', (err, data) => {
  let moves = data.toString().split(',')
  let programs = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p']
  let previouslySeen = {
    'abcdefghijklmnop': 0
  }

  let j = 0
  let skipped = true
  let pbar = new ProgressBar('Dancing [:bar] :current/:total', { total: 1000000000 })
  let start = process.hrtime()[0]

  while (j < 1000000000) {
    for (let i = 0; i < moves.length; i++) {
      let move = moves[i]

      let type = move[0]
      let moveData = move.substr(1).split('/')
      let p1index, p2index, p1

      switch (type) {
        case 's':
          let spin = programs.splice(-(parseInt(moveData[0])))
          programs = spin.concat(programs)
          break
        case 'x':
          p1index = parseInt(moveData[0])
          p2index = parseInt(moveData[1])

          p1 = programs[p1index]
          programs[p1index] = programs[p2index]
          programs[p2index] = p1
          break
        case 'p':
          p1index = programs.indexOf(moveData[0])
          p2index = programs.indexOf(moveData[1])

          p1 = programs[p1index]
          programs[p1index] = programs[p2index]
          programs[p2index] = p1
          break
      }
    }

    let order = programs.join('')
    j++
    pbar.tick()
    if (j % 10000 === 0) {
      let elapsed = process.hrtime()[0] - start

      let timePerIteration = j / elapsed
      let estimatedTimeRemaining = timePerIteration * (1000000000 - j)
      console.log('Completed ' + j + ' in ' + elapsed.toFixed(0) + 's, estimated time remaining: ' + estimatedTimeRemaining.toFixed(0) + 's')
    }

    if (!skipped) {
      if (order in previouslySeen) {
        console.log('Will skip, cur pos is ' + j + ', last seen after ' + previouslySeen[order])
        let loopSize = j - previouslySeen[order]
        console.log('Loop size is ' + loopSize)

        let numLoops = Math.floor((1000000000 - j) / loopSize)
        let skip = numLoops * loopSize
        j += skip
        console.log('Skipping ' + skip + ' runs, now at ' + j)
        skipped = true
      } else {
        previouslySeen[order] = j
      }
    }
  }

  console.log('Program order: ' + programs.join(''))
})
