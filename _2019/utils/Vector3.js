class Vector3 {
  constructor(x, y, z) {
    this.x = x;
    this.y = y;
    this.z = z;
  }

  getMagnitude() {
    return Math.abs(this.x) + Math.abs(this.y) + Math.abs(this.z);
  }

  translateBy(vector) {
    this.x += vector.x;
    this.y += vector.y;
    this.z += vector.z;
  }
}

module.exports = Vector3;
