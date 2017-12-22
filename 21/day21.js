var fs = require('fs')
var _ = require('lodash')

var reverseString = (str) => {
  return str.split('').reverse().join('')
}

var addTransforms = (transforms, source, target) => {
  if (target.length === 3) {
    let stack = [source]

    while (stack.length) {
      let curKey = stack.pop()

      if (!transforms[curKey]) {
        transforms[curKey] = target

        // Flip on horizontal axis
        let hozFlipKey = curKey.substr(2, 2) + curKey.substr(0, 2)
        stack.push(hozFlipKey)

        // Flip on vertical axis
        let verFlipKey = curKey.charAt(1) + curKey.charAt(0) + curKey.charAt(3) + curKey.charAt(2)
        stack.push(verFlipKey)

        // Rotate
        let rotKey = curKey.charAt(2) + curKey.charAt(0) + curKey.charAt(3) + curKey.charAt(1)
        stack.push(rotKey)
      }
    }
  } else {
    let stack = [source]

    while (stack.length) {
      let curKey = stack.pop()

      if (!transforms[curKey]) {
        transforms[curKey] = target

        // Flip on horizontal axis
        let hozFlipKey = curKey.substr(6, 3) + curKey.substr(3, 3) + curKey.substr(0, 3)
        stack.push(hozFlipKey)

        // Flip on vertical axis
        let verFlipKey = reverseString(curKey.substr(0, 3)) + reverseString(curKey.substr(3, 3)) + reverseString(curKey.substr(6, 3))
        stack.push(verFlipKey)

        // Rotate
        let rotKey = curKey.charAt(6) + curKey.charAt(3) + curKey.charAt(0) + curKey.charAt(7) + curKey.charAt(4) + curKey.charAt(1) + curKey.charAt(8) + curKey.charAt(5) + curKey.charAt(2)
        stack.push(rotKey)
      }
    }
  }
}

var squarify = (programString) => {
  let squares = []

  if (programString.length % 2 === 0) {
    // Divide into 2x2 squares
    let lineLength = Math.sqrt(programString.length)
    let squaresPerLine = lineLength / 2
    let numSquares = Math.pow(squaresPerLine, 2)

    for (let i = 0; i < numSquares; i++) {
      let start = (Math.floor(i / squaresPerLine) * lineLength) + i * 2
      squares.push([programString.substr(start, 2), programString.substr(start + lineLength, 2)])
    }
  } else {
    // Divide into 3x3 squares
    let lineLength = Math.sqrt(programString.length)
    let squaresPerLine = lineLength / 3
    let numSquares = Math.pow(squaresPerLine, 2)

    for (let i = 0; i < numSquares; i++) {
      let start = (Math.floor(i / squaresPerLine) * lineLength * 2) + i * 3
      squares.push([programString.substr(start, 3), programString.substr((start) + lineLength, 3), programString.substr((start) + (lineLength * 2), 3)])
    }
  }

  return squares
}

var joinSquares = (squares) => {
  let squaresPerLine = Math.sqrt(squares.length)
  let squareSize = squares[0].length

  let joinedString = ''

  while (squares.length) {
    if (squares.length < squaresPerLine) {
      console.log('Expected ' + squaresPerLine + ', found ' + squares.length)
    }
    let lineSquares = squares.splice(0, squaresPerLine)

    for (let i = 0; i < squareSize; i++) {
      joinedString = joinedString.concat((_.map(lineSquares, (s) => { return s[i] })).join(''))
    }
  }

  return joinedString
}

fs.readFile('conversions.txt', (err, data) => {
  if (err) {
    console.warn(err.message)
    process.exit()
  }

  let lines = data.toString().split('\n')
  let transforms = {}

  for (let i = 0; i < lines.length; i++) {
    let line = lines[i].split(' => ')

    let transform = line[1].split('/')

    addTransforms(transforms, line[0].split('/').join(''), transform)
  }

  console.log('Transforms complete. Num transforms: ' + _.size(transforms))

  let program = '.#...####'

  console.log('Start at ' + program)

  for (let i = 0; i < 18; i++) {
    let programSquares = squarify(program)

    for (let j = 0; j < programSquares.length; j++) {
      let transform = transforms[programSquares[j].join('')]

      if (transform === undefined) {
        console.warn('Undefined transform for ' + programSquares[j].join(''))
      } else {
        console.log('Transformed ' + programSquares[j].join('') + ' to ' + transform.join(''))
      }

      programSquares[j] = transform
    }

    program = joinSquares(programSquares)
    console.log('Got program ' + program + ' after ' + i)
    console.log('---')

    if (i === 4) {
      console.log('After 5: ' + program)

      let numOn = program.split('#').length - 1

      console.log('Num on pixels after 5: ' + numOn)
    }
  }

  console.log('After 18: ' + program)

  let numOn = program.split('#').length - 1

  console.log('Num on pixels after 18: ' + numOn)
})
