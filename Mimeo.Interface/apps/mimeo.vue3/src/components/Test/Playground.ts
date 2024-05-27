
// 5/25/2024 - ultra-fucking basics: spread notation - Observe syntax that uses brackets with a Union of parameters
//
function testFunc0001(...args: [string]): void {}
function testFunc0002(...args: [string | number | Boolean]): void {}

// Typescript Enlightenment
//
// Step 1 - we create our "little machine" which can extract return type of the function passed to it
//
type ReturnTypeX<T> = T extends (...args: any[]) => infer R ? R : never;

// Step 2 - here we create a concrete function
//
function exampleFunction() {
  return 123;
}

// Step 3 - applying our friend "typeof" to the exampleFunction, we then set FunctionReturnType
//
type FunctionReturnType = ReturnTypeX<typeof exampleFunction>; // number

// Naturally, then we want to know why "infer" isn't chastised by the compiler for not using the brackets, either?
//
type InferEmitType<T> = T extends (...args: infer P) => void
  ? (...args: P) => void
  : never;

// A very helpful twist: we can use parenthesis before an array in our type constraint to stuff "type logic".
//
type ElementType3<T extends (number|string)[]> = {
  ArrayStuff: T
};

// Well done!
//
let myFirstFlight: ElementType3<number[]> = { ArrayStuff: [1, 2, 3, 4] };


// This is a "type machine". The condition clause tests that an array is being passed, and if so, it returns the array type.
//
type ArrayTypeExtractor<T> = T extends (infer U)[] ? U : never;
type MyArrayType = ArrayTypeExtractor<number>

// Plain old Hashmap
//
type WhatThis = {
  [dfdfndex: number]: string; // It doesn't matter that we call it "dfdfndex", it's simply a placehold for object key
}

let g: WhatThis = {
  3: "1",
};


let Concrete = { x: 3, y: "yo!"};
type Pick2<T, K extends keyof T> = {
  [P in K]: T[P];
};
type TypeP = Pick2<typeof Concrete, "x">;
let Impl: TypeP = { x: 543 };



interface IdLabel {
  id: number /* some fields */;
}
interface NameLabel {
  name: string /* other fields */;
}

// These are function overloads
//
function createLabel(id: number): IdLabel;
function createLabel(name: string): NameLabel;
function createLabel(nameOrId: string | number): IdLabel | NameLabel;
function createLabel(nameOrId: string | number): IdLabel | NameLabel {
  if (typeof nameOrId === "string") {
  }
  if (typeof nameOrId === "number") {
  }
  return null as any;
}

defineEmits<typeof createLabel>

export declare function defineEmits<T extends ((...args: any[]) => any) | Record<string, any[]>>(): T extends (...args: any[]) => any ? T : ShortEmits<T>;

