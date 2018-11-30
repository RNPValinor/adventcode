var fs = require('fs')

let getDistance = (pos) => {
  let distance
  if ((pos.northEast >= 0 && pos.northWest >= 0) || (pos.northEast <= 0 && pos.northWest <= 0)) {
    distance = Math.abs(pos.north) + Math.max(Math.abs(pos.northEast), Math.abs(pos.northWest))
  } else {
    let ne = Math.abs(pos.northEast)
    let nw = Math.abs(pos.northWest)
    let newNorth = Math.max(0, Math.abs(pos.north) - Math.min(ne, nw))
    distance = ne + nw + newNorth
  }
  return distance
}

fs.readFile('hex-moves.txt', (err, data) => {
  let moves = data.toString().split(',')
  let curPos = {
    north: 0,
    northEast: 0,
    northWest: 0
  }
  let maxDistance = 0

  for (let i = 0; i < moves.length; i++) {
    switch (moves[i]) {
      case 'n':
        curPos.north++
        break
      case 's':
        curPos.north--
        break
      case 'nw':
        curPos.northWest++
        break
      case 'se':
        curPos.northWest--
        break
      case 'ne':
        curPos.northEast++
        break
      case 'sw':
        curPos.northEast--
        break
    }

    maxDistance = Math.max(maxDistance, getDistance(curPos))
  }

  console.log('Final position is north: ' + curPos.north + ', northEast: ' + curPos.northEast + ', northWest: ' + curPos.northWest)

  let distance = getDistance(curPos)

  console.log('Distance is ' + distance)

  console.log('Max distance is ' + maxDistance)
})
