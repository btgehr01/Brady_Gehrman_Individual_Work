 ; Lab4
 ;
 ; Created: 3/24/2018 4:15:16 AM
 ; Author : Eugene Rockey

		 .org 0				;student discuss interrupts and vector table in report
		 jmp RESET			;student discuss in report
		 jmp INT0_H			;student discuss in report
		 jmp INT1_H			;student discuss in report
		 jmp PCINT0_H			;student discuss in report
		 jmp PCINT1_H			;student discuss in report
		 jmp PCINT2_H			;student discuss in report
		 jmp WDT			;student discuss in report
		 jmp TIM2_COMPA			;student discuss in report
		 jmp TIM2_COMPB			;student discuss in report
		 jmp TIM2_OVF			;student discuss in report
		 jmp TIM1_CAPT			;student discuss in report
		 jmp TIM1_COMPA			;student discuss in report
		 jmp TIM1_COMPB			;student discuss in report
		 jmp TIM1_OVF			;student discuss in report
		 jmp TIM0_COMPA			;student discuss in report
		 jmp TIM0_COMPB			;student discuss in report
		 jmp TIM0_OVF			;student discuss in report
		 jmp SPI_TC			;student discuss in report
		 jmp USART_RXC			;student discuss in report
		 jmp USART_UDRE			;student discuss in report
		 jmp USART_TXC			;student discuss in report
		 jmp ADCC			;student discuss in report
		 jmp EE_READY			;student discuss in report
		 jmp ANA_COMP			;student discuss in report
		 jmp TWI			;student discuss in report
		 jmp SPM_READY			;student discuss in report



RESET:	;Initialize the ATMega328P chip for the THIS embedded application.
		;initialize PORTB for Output
		cli
		ldi	r16,0xFF				;PB1 or OC1A Output
		out	DDRB,r16
;initialize and start Timer A, compare match, interrupt enabled
		ldi	r16,0xC0			;set OC to compare match set output to high level
		sts TCCR1A,r16			;stores 11000000 to set OC1A/OC1B on Compare Match (Set output to high level).
		ldi r16,0x04			;set clock prescaler
		sts TCCR1B,r16			;stores 00000100 to set CS12 high, to set the timer prescalar, 256 is the prescalar
		ldi r16,0x80			;force output compare 1a, set PB1 high
		sts TCCR1C,r16			;stores 10000000 to force output compare 1a to be on (forces immediate compare match)
		ldi r16,0x40			;load 01000000 into register 16
		sts TCCR1A,r16			;store 01000000 to set 0C1B to high, this clears OC1A, sets to low level 
		ldi	r18,0x0B			;load 00001011 into r18 (for high register 11*256 = 2816) 
		ldi r17,0xB8			;load 10111000 into r17, 184
		lds r16,TCNT1L			;load TCNT1L, timer counter low value, into R16, should be zero at beginning 
		add r17,r16				;adds TCNT1L, timer counter low value, to 10111000
		lds r16,TCNT1H			;load TCNT1H, timer counter high value, to R16, should be zero at beginning 
		adc r18,r16				;adds TCNT1H, timer counter high value, to 00001011
		sts OCR1AH,r18			;stores timer counter high value addition to OCR1AH, 
		sts OCR1AL,r17			;stores timer counter low value addition to OCR1AL, OCR1AH and OCR1AL are continously compared to the counter value TCNT1
								;A match can be used to generate an Output Compare interrupt, or to generate a waveform output on the OC1A pin (port b1).
		ldi r19,0				;load 0 into r19
		ldi r16,0x02			;load 00000010 into r16
		sts TIMSK1,r16			;stores 00000010 into TIMSK1, the Timer/Counter 1 Output Compare A Match interrupt is enabled 
		out TIFR1,r16			;outputs 00000010 into TIFR1 ,the Timer Interrupt Flag Register bit 1 is set This flag (A) is set in the timer clock cycle after the counter (TCNT1) value matches the Output Compare Register A (OCR1A)
		sei						;set back up interupts 
here:	rjmp here
		
INT0_H:
		nop			;external interrupt 0 handler
		reti
INT1_H:
		nop			;external interrupt 1 handler
		reti
PCINT0_H:
		nop			;pin change interrupt 0 handler
		reti
PCINT1_H:
		nop			;pin change interrupt 1 handler
		reti
PCINT2_H:
		nop			;pin change interrupt 2 handler
		reti
WDT:
		nop			;watch dog time out handler
		reti
TIM2_COMPA:
		nop			;TC 2 compare match A handler
		reti
TIM2_COMPB:
		nop			;TC 2 compare match B handler
		reti
TIM2_OVF:
		nop			;TC 2 overflow handler
		reti
TIM1_CAPT:
		nop			;TC 1 capture event handler
		reti
TIM1_COMPA:			;TC 1 compare match A handler
		sbrc	r19,0				;skip if 0th bit in register is cleared
		rjmp	ONE					;jump to ONE below if 0th bit in register is not cleared
		ldi		r17,0xE8			;loads 11101000 into r17, 232
		ldi		r18,0x0B			;loads 00001011 into r18, 11 which is really 11, total of 3048
		ldi		r19,1				;loads 00000001 into r19, enables the "ONE" subroutine, sets port b1 to a 1
		rjmp	BEGIN				;jump to BEGIN	
ONE:	ldi		r17,0xE8			;loads 11101000 into r17
		ldi		r18,0x0B			;loads 00001011 into r18
		ldi		r19,0				;loads 00000000 into r19
BEGIN:	lds		r16,OCR1AL			;loads output compare register 1a low byte into r16
		add		r17,r16				;adds output compare register 1a low byte value to 11101000
		lds		r16,OCR1AH			;loads output compare register 1a high byte into r16
		adc		r18,r16				;adds output compare register 1a high byte value to 00001011
		sts		OCR1AH,r18			;stores timer counter high value addition to OCR1AH
		sts		OCR1AL,r17			;stores timer counter low value addition to OCR1AL, OCR1AH and OCR1AL are continously compared to the counter value TCNT1, 
		reti						;return from interupt  
TIM1_COMPB:
		nop			;TC 1 compare match B handler
		reti
TIM1_OVF:
		nop			;TC 1 overflow handler
		reti
TIM0_COMPA:
		nop			;TC 0 compare match A handler
		reti
TIM0_COMPB:			
		nop			;TC 1 compare match B handler
		reti
TIM0_OVF:
		nop			;TC 0 overflow handler
		reti
SPI_TC:
		nop			;SPI Transfer Complete
		reti
USART_RXC:
		nop			;USART receive complete
		reti
USART_UDRE:
		nop			;USART data register empty
		reti
USART_TXC:
		nop			;USART transmit complete
		reti
ADCC:
		nop			;ADC conversion complete
		reti
EE_READY:
		nop			;EEPROM ready
		reti
ANA_COMP:
		nop			;Analog Comparison complete 
		reti
TWI:
		nop			;I2C interrupt handler
		reti
SPM_READY:
		nop			;store program memory ready handler
		reti		

