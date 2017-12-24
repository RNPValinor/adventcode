var fs = require('fs')
var _ = require('lodash')

var maxStrength = 0
var maxLength = 0

var buildBridges = (strength, totalStrength, visited, ports) => {
  let nextPorts = ports[strength]
  let foundNext = false

  for (let i = 0; i < nextPorts.length; i++) {
    let nextPort = nextPorts[i]

    if (visited.indexOf(nextPort) === -1) {
      foundNext = true

      let nextStrength = nextPort[0] === strength ? nextPort[1] : nextPort[0]
      let nextTotalStrength = totalStrength + nextPort[1] + nextPort[0]
      let nextVisited = _.clone(visited)
      nextVisited.push(nextPort)

      buildBridges(nextStrength, nextTotalStrength, nextVisited, ports)
    }
  }

  if (!foundNext) {
    if (visited.length > maxLength) {
      maxLength = visited.length
      maxStrength = totalStrength
    } else if (visited.length === maxLength) {
      if (totalStrength > maxStrength) {
        maxStrength = totalStrength
      }
    }
  }
}

fs.readFile('24/ports.txt', (err, data) => {
  if (err) {
    console.error(err.message)
    process.exit()
  }

  let lines = data.toString().split('\n')

  // Map of PortVal -> Port. Ports are duplicated (to appear in each PortVal for left and right values)
  let ports = {}

  for (let i = 0; i < lines.length; i++) {
    let port = lines[i].split('/')
    port[0] = parseInt(port[0])
    port[1] = parseInt(port[1])

    if (!ports[port[0]]) {
      ports[port[0]] = []
    }
    if (!ports[port[1]]) {
      ports[port[1]] = []
    }

    ports[port[0]].push(port)
    ports[port[1]].push(port)
  }

  for (let i = 0; i < ports[0].length; i++) {
    let port = ports[0][i]

    let nextStrength = port[0] === 0 ? port[1] : port[0]
    let totalStrength = nextStrength
    let visited = [port]

    buildBridges(nextStrength, totalStrength, visited, ports)
  }

  console.log('Got max strength: ' + maxStrength)
})
