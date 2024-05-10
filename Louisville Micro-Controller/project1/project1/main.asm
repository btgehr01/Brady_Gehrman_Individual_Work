;
; project1.asm
;
; Created: 5/30/2021 7:42:15 PM
; Author : Brady Gehrman
;

//Bit Instructions
.cseg ;FLASH code segment

.org 0x0

start: ldi r26,0x00

ldi r27,0x01

ldi r30,0xAC

lsl r30 ;Logical Shift Left, shifts every number in the binary value to the left one ;digit (EX: 010 would become 100) - flags - H,S,V,C (Half carry, Signed, ;Two's Complement, and Carry)

lsr r30 ;Logical Shift Right, shifts every number in the binary value to the right ;one digit (EX: 010 would become 001) - flags -

asr r30 ;Arithmetic Shift Right, shifts every number in the binary value to the ;right a chosen number of digits, in this case one - flags - none

bset 2 ;Sets the second pin value to high (1) - flags - N (Negative)

bclr 2 ;Sets the second pin value to low (0) - flags - none

brmi here

st X+,r30 ;Stores the value at r30 in the SRAM memory location equal to the X ;register (0x0100) then post increments the X register by 1 - flags - none

st X+,r30 ;Stores the value at r30 in the SRAM memory location equal to the X ;register (0x0101) then post increments the X register by 1 - flags - none

st X+,r30 ;Stores the value at r30 in the SRAM memory location equal to the X ;register (0x0102) then post increments the X register by 1 - flags - none

here: jmp here

.exit
