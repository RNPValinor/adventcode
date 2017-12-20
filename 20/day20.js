var fs = require('fs')

fs.readFile('positions.txt', (err, data) => {
  if (err) {
    process.exit()
  }

  let lines = data.toString().split('\r\n')
  let points = []
  let lowestAcceleration = {
    acc: 10000,
    points: []
  }

  for (let i = 0; i < lines.length; i++) {
    let pointData = lines[i].split(', ')

    let position = pointData[0].substring(3, pointData[0].length - 1).split(',')
    let velocity = pointData[1].substring(3, pointData[1].length - 1).split(',')
    let acceleration = pointData[2].substring(3, pointData[2].length - 1).split(',')

    let point = {
      id: i,
      pos: {
        x: parseInt(position[0]),
        y: parseInt(position[1]),
        z: parseInt(position[2])
      },
      vel: {
        x: parseInt(velocity[0]),
        y: parseInt(velocity[1]),
        z: parseInt(velocity[2])
      },
      acc: {
        x: parseInt(acceleration[0]),
        y: parseInt(acceleration[1]),
        z: parseInt(acceleration[2])
      }
    }

    points.push(point)

    let accVal = Math.abs(point.acc.x) + Math.abs(point.acc.y) + Math.abs(point.acc.z)

    if (accVal < lowestAcceleration.acc) {
      lowestAcceleration.acc = accVal
      lowestAcceleration.points = [point]
    } else if (accVal === lowestAcceleration.acc) {
      lowestAcceleration.points.push(point)
    }
  }

  console.log('Lowest acc is ' + lowestAcceleration.acc + ', num points: ' + lowestAcceleration.points.length)
  console.log('Point IDs are ' + lowestAcceleration.points[0].id + ', ' + lowestAcceleration.points[1].id)

  // 119: p=<-3329,585,1447>, v=<98,-17,-8>, a=<0,0,-2>
  // 170: p=<430,891,209>, v=<3,-10,-8>, a=<-1,-1,0>
})
