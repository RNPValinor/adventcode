var fs = require('fs')
var _ = require('lodash')

fs.readFile('graph.txt', (err, data) => {
  let graph = data.toString().split('\r\n')
  graph = _.map(graph, (l) => l.split(''))
  let numNonEmpty = _.sumBy(graph, (l) => { return (_.filter(l, (c) => { return c !== ' ' })).length })
  console.log('Num non empty: ' + numNonEmpty)

  let position = {
    row: 0,
    col: graph[0].indexOf('|')
  }

  let direction = {
    right: 0,
    down: 1
  }

  let letters = []
  let numSteps = 1 // Start as 1, as moving onto the initial | counts as 1 step

  do {
    let next = graph[position.row + direction.down][position.col + direction.right]

    if (next === ' ') {
      if (graph[position.row][position.col] === '+') {
        if (direction.right !== 0) {
          // Was going left/right, now going up/down
          direction.right = 0
          direction.down = graph[position.row + 1][position.col] !== ' ' ? 1 : -1
        } else {
          // Was going up/down, now going left/right
          direction.down = 0
          direction.right = graph[position.row][position.col + 1] !== ' ' ? 1 : -1
        }
      } else {
        // No more pipe, but we're not on a corner ('+'). Must be at the end, so we're done
        console.log('Letters are ' + letters.join(''))
        console.log('Num steps: ' + numSteps)
        process.exit()
      }
    } else {
      if (next !== '+' && next !== '|' && next !== '-') {
        // Next is not a space, nor a pipe icon - must be a letter
        letters.push(next)
      }

      position.row += direction.down
      position.col += direction.right
      numSteps++
    }
  } while (graph[position.row][position.col] !== ' ')
})
