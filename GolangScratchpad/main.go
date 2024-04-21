package main

/*
import (
    "askscratchpad/golang/ryang"
    "fmt"
)

func main() {
    rng := ryang.RyandomNumberGenerator{}

    fmt.Println(rng.Ryandomize(5, 10))
    fmt.Println(rng.RyandomizeUpperBound(10))
}
*/

import (
    "fmt"
)

func main() {
    a := A{}
    angka := a.SomeFunc()
    fmt.Println(angka)
}

type A struct {}

func (a *A) SomeFunc() (int32) {
    return 46692016
}