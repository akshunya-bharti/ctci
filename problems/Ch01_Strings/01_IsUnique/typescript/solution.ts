import * as fs from "fs";

const input = fs.readFileSync(0, "utf-8").trim().split("\n");
const n = parseInt(input[0]);
console.log(n * n);