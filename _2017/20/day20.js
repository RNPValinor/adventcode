var fs = require('fs')
var _ = require('lodash')

var solveQuad = (a, b, c) => {
  let a2 = 2 * a
  let ac = 4 * a * c
  let dis = b * b
  dis -= ac

  if (dis < 0) {
    return false
  } else {
    let disRoot = Math.sqrt(dis)
    let x1 = -b + disRoot
    x1 /= a2
    let x2 = -b - disRoot
    x2 /= a2

    if (!isNaN(x1) && !isNaN(x2) && isFinite(x1) && isFinite(x2)) {
      let vals = []

      if (x1 > 0) {
        vals.push(x1)
      }
      if (x2 > 0) {
        vals.push(x2)
      }
      if (vals.length) {
        return vals
      }
    }

    return false
  }
}

var getCollisions = (points) => {
  let collisions = {}

  for (let i = 0; i < points.length; i++) {
    for (let j = i + 1; j < points.length; j++) {
      let point1 = points[i]
      let point2 = points[j]

      let x2term = point1.acc.x - point2.acc.x
      let x1term = 2 * (point1.vel.x - point2.vel.x) + (point1.acc.x - point2.acc.x)
      let x0term = 2 * (point1.pos.x - point2.pos.x)

      let y2term = point1.acc.y - point2.acc.y
      let y1term = 2 * (point1.vel.y - point2.vel.y) + (point1.acc.y - point2.acc.y)
      let y0term = 2 * (point1.pos.y - point2.pos.y)

      let z2term = point1.acc.z - point2.acc.z
      let z1term = 2 * (point1.vel.z - point2.vel.z) + (point1.acc.z - point2.acc.z)
      let z0term = 2 * (point1.pos.z - point2.pos.z)

      let xRoots = solveQuad(x2term, x1term, x0term)
      let yRoots = false
      let zRoots = false

      if (xRoots) {
        yRoots = solveQuad(y2term, y1term, y0term)

        if (yRoots) {
          zRoots = solveQuad(z2term, z1term, z0term)
        }
      }

      if (xRoots && yRoots && zRoots) {
        xRoots = _.map(xRoots, (r) => { return Math.round(r) })
        yRoots = _.map(yRoots, (r) => { return Math.round(r) })
        zRoots = _.map(zRoots, (r) => { return Math.round(r) })

        let t = _.intersection(xRoots, yRoots, zRoots)

        if (t.length) {
          let time = t[0]

          // Validate actual collision time, not error caused by inexact quadratic solving
          let p1CollisionPos = {
            x: point1.pos.x + time * point1.vel.x + ((time * (time + 1)) / 2) * point1.acc.x,
            y: point1.pos.y + time * point1.vel.y + ((time * (time + 1)) / 2) * point1.acc.y,
            z: point1.pos.z + time * point1.vel.z + ((time * (time + 1)) / 2) * point1.acc.z
          }

          let p2CollisionPos = {
            x: point2.pos.x + time * point2.vel.x + ((time * (time + 1)) / 2) * point2.acc.x,
            y: point2.pos.y + time * point2.vel.y + ((time * (time + 1)) / 2) * point2.acc.y,
            z: point2.pos.z + time * point2.vel.z + ((time * (time + 1)) / 2) * point2.acc.z
          }

          if (_.isEqual(p1CollisionPos, p2CollisionPos)) {
            if (!collisions[time]) {
              collisions[time] = []
            }

            collisions[time].push(points[i].id)
            collisions[time].push(points[j].id)
          }
        }
      }
    }
  }

  return collisions
}

fs.readFile('positions.txt', (err, data) => {
  if (err) {
    process.exit()
  }

  let lines = data.toString().split('\n')
  let points = {}
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

    points[point.id] = point

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

  let startPoints = _.cloneDeep(points)

  for (let t = 0; t < 150; t++) {
    let newPositions = {}
    let pointValues = _.values(points)

    for (let i = 0; i < pointValues.length; i++) {
      let point = pointValues[i]

      point.vel.x += point.acc.x
      point.vel.y += point.acc.y
      point.vel.z += point.acc.z

      point.pos.x += point.vel.x
      point.pos.y += point.vel.y
      point.pos.z += point.vel.z

      let pointPos = point.pos.x + ',' + point.pos.y + ',' + point.pos.z
      if (newPositions[pointPos]) {
        newPositions[pointPos].push(point)
      } else {
        newPositions[pointPos] = [point]
      }
    }

    let collisions = _.values(newPositions)
    let deletedPoints = []

    for (let i = 0; i < collisions.length; i++) {
      let posPoints = collisions[i]

      if (posPoints.length > 1) {
        for (let i = 0; i < posPoints.length; i++) {
          delete points[posPoints[i].id]
          deletedPoints.push(posPoints[i].id)
        }
      }
    }

    if (deletedPoints.length) {
      console.log('At time ' + t + ' points ' + deletedPoints.join(', ') + ' collided')
    }
  }

  console.log('Ran 150 times, points left: ' + _.size(points))

  // Process below should be a general case solution, but it's broken

  points = startPoints

  let collisions = getCollisions(_.values(points))

  while (_.size(collisions)) {
    let collisionTimes = (_.keys(collisions)).sort((a, b) => { return a - b })
    console.log('Collision times are ' + collisionTimes.join(', '))
    let i = 0

    while (i < collisionTimes.length && _.every(collisions[collisionTimes[i]], (id) => { return !!points[id] })) {
      let pointsToRemove = _.uniq(collisions[collisionTimes[i]])
      console.log('Deleting at collision time ' + collisionTimes[i] + ', points: ' + pointsToRemove.join(', '))

      for (let j = 0; j < pointsToRemove.length; j++) {
        delete points[pointsToRemove[j]]
      }

      i++
    }

    if (i < collisionTimes.length) {
      console.log('Stuck on ' + _.values(collisions[collisionTimes[i]]).join(', ') + ' at collision time ' + collisionTimes[i])
    }

    console.log('Removed points, ' + _.size(points) + ' points remain')
    collisions = getCollisions(_.values(points))
  }
})

// (566, 777)
// !575
// !671
