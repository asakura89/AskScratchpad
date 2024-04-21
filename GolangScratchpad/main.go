package main

import (
    "askscratchpad/golang/ryang"
    "fmt"
)

func main() {
    fmt.Println(ryang.Ryandomize(5, 10))
    fmt.Println(ryang.RyandomizeUpperBound(10))
}
