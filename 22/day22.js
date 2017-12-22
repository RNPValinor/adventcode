var fs = require('fs')

var numInfected = 0

var flipInfection = (map, pos) => {
  // Nodes in map can be:
  //   False - clean
  //   'W' - weakened
  //   'I' - infected
  //   'F' - flagged

  if (map[pos.x]) {
    let curVal = map[pos.x][pos.y]
    let nextVal

    if (!curVal) {
      // Was clean, now weakened
      nextVal = 'W'
    } else if (curVal === 'W') {
      // Was weakened, now infected
      nextVal = 'I'
      numInfected++
    } else if (curVal === 'I') {
      // Was infected, now flagged
      nextVal = 'F'
    } else if (curVal === 'F') {
      // Was flagged, now cleaned
      nextVal = false
    } else {
      console.error('Invalid square state ' + curVal + ' at (' + pos.x + ', ' + pos.y + ')')
      process.exit()
    }

    map[pos.x][pos.y] = nextVal
  } else {
    map[pos.x] = {}
    map[pos.x][pos.y] = 'W'
  }
}

var turn = (map, pos, dir) => {
  let state = map[pos.x] && map[pos.x][pos.y]

  if (!state) {
    // Clean, turn left
    if (dir.down !== 0) {
      dir.right = dir.down
      dir.down = 0
    } else {
      dir.down = 0 - dir.right
      dir.right = 0
    }
  } else if (state === 'I') {
    if (dir.down !== 0) {
      dir.right = 0 - dir.down
      dir.down = 0
    } else {
      dir.down = dir.right
      dir.right = 0
    }
  } else if (state === 'F') {
    dir.right = 0 - dir.right
    dir.down = 0 - dir.down
  } else if (state === 'W') {
    // Do nothing when weakened
  }
}

var move = (pos, dir) => {
  if (dir.down !== 0) {
    pos.y += dir.down
  } else {
    pos.x += dir.right
  }
}

fs.readFile('map.txt', (err, data) => {
  if (err) {
    console.error(err.message)
    process.exit()
  }

  let lines = data.toString().split('\n')
  let map = {}

  let subY = Math.floor(lines.length / 2)
  let subX = Math.floor(lines[0].length / 2)

  for (let y = 0; y < lines.length; y++) {
    let line = lines[y]

    for (let x = 0; x < line.length; x++) {
      if (line[x] === '#') {
        let xMap = map[x - subX]

        if (!xMap) {
          xMap = {}
          map[x - subX] = xMap
        }

        xMap[y - subY] = 'I'
      }
    }
  }

  let pos = {
    x: 0,
    y: 0
  }

  let dir = {
    down: -1,
    right: 0
  }

  for (let i = 0; i < 10000000; i++) {
    turn(map, pos, dir)

    flipInfection(map, pos)

    move(pos, dir)

    // console.log('Now at (' + pos.x + ', ' + pos.y + '), travelling (' + dir.down + ', ' + dir.right + ')')
  }

  console.log('Num infected: ' + numInfected)
})
