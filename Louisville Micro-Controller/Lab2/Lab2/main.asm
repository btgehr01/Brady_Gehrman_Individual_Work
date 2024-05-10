;
; Lab2.asm
;
; Created: 6/14/2021 9:26:27 PM
; Author : Brady Gehrman
;
;******************************
;* Declare Variables
;******************************

ldi R16, 0x03
mov R1, R16
ldi R16, 0x25
mov R0, R16
clr R2
ldi R16, 0x7A
mov R3,R16
clr R6
clr R5
clr R4
loop1:
lsr R3
brcc Loop2
add R4, R0
add R5, R1
add R6, R2
loop2:
lsl R0
rol R1
rol R2
tst R3
brne Loop1
