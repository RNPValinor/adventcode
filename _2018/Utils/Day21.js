r0 = 0;
r1 = 0;
r2 = 0;
r3 = 0;
r4 = 0;
r5 = 0;

do {
  r2 = r1 | 65536;
  r1 = 7902108;

  do {
    r5 = r2 & 255;
    r1 = r1 + r5;
    r1 = r1 & 16777215;
    r1 = r1 * 65899;
    r1 = r1 & 16777215;
    r5 = 256 > r2 ? 1 : 0;

    if (r5 == 1) {
      break;
    } else {
      r5 = 0;
      r3 = 256;
      r3 = r3 > r2 ? 1 : 0;

      if (r3 == 0) {
        r5 = r5 + 1;
        r4 = 17;
      }

      r2 = r5;
    }
  } while (true);

  console.log("Got r1 of " + r1);

  r5 = r1 == r0 ? 1 : 0;
} while (r5 == 0);
