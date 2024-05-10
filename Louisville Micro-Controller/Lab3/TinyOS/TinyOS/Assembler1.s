
/*
 * Assembler1.s
 *
 * Created: 7/6/2021 6:40:59 PM
 *  Author: Brady Gehrman
 */ 

 // Lab3P1.s
 //
 // Created: 1/30/2018 4:15:16 AM
 // Author : Eugene Rockey
 // Copyright 2018, All Rights Reserved


.section ".data"					//Working in the data registers 
.equ	DDRB,0x04					//Data Direction register for port B 
.equ	DDRD,0x0A					//Data Direction register for port D 
.equ	PORTB,0x05					//Port B  
.equ	PORTD,0x0B					//Port D 
.equ	U2X0,1						//This is the USART port 
.equ	UBRR0L,0xC4					//This is for the USART Baud rate (Low // bits) 
.equ	UBRR0H,0xC5					//This is for the USART Baud rate (High // bits) 
.equ	UCSR0A,0xC0					//Control register for USART 
.equ	UCSR0B,0xC1					//Control register for USART 
.equ	UCSR0C,0xC2					//Control register for USART  
.equ	UDR0,0xC6					//I/O data register (for reading and writing) 
.equ	RXC0,0x07					//Receives information 
.equ	UDRE0,0x05					//I/O data register (for reading and writing) 
.equ	ADCSRA,0x7A					//Analog to Digital converter register A 
.equ	ADMUX,0x7C					//Analog to digital multiplexer 
.equ	ADCSRB,0x7B					//Analog to digital converter register B 
.equ	DIDR0,0x7E					//Store digital value 
.equ	DIDR1,0x7F					//Store digital value 
.equ	ADSC,6						//Analog to digital single conversion 
.equ	ADIF,4						//Analog to digital free running 
.equ	ADCL,0x78					//Analog to digital low bit 
.equ	ADCH,0x79					//Analog to digital high bit 
.equ	EECR,0x1F					//EEPROM control register 
.equ	EEDR,0x20					//EEPROM data register 
.equ	EEARL,0x21					//EEPROM address register low bit 
.equ	EEARH,0x22					//EEPROM address register high bit 
.equ	EERE,0						//EEPROM Read enable 
.equ	EEPE,1						//EEPROM program enable 
.equ	EEMPE,2						//EEPROM manual program enable 
.equ	EERIE,3						//EEPROM read interrupr enable 

  

.global HADC						//High bits for the ADC reading 
.global LADC						//Low bits for the ADC reading 
.global ASCII						//Represents the string or characters to be displayed 
.global DATA						//Any data return value 

.set	temp,0						// used for displaying temperature on the LCD 

.section ".text"					//student comment here 

.global Mega328P_Init 

Mega328P_Init: 
		ldi	r16,0x07			;PB0(R*W),PB1(RS),PB2(E) as fixed outputs 

		out	DDRB,r16			//initialize data register for port b 

		ldi	r16,0				//student comment here 

		out	PORTB,r16			//Initalize Port B 

		out	U2X0,r16			;initialize UART, 8bits, no parity, 1 stop, 9600 

		ldi	r17,0x0				//Set register 17 and 16 to 0x0067 

		ldi	r16,0x67			//Set register 17 and 16 to 0x0067 

		sts	UBRR0H,r17			//Baud rates high and low equal to r17 and 16 (103) 

		sts	UBRR0L,r16			//Baud rates high and low equal to r17 and 16 (103) 

		ldi	r16,24				// Bits 2 and 5 high 

		sts	UCSR0B,r16			//Setting character size and Data Register Interrupt flag 

		ldi	r16,6				// Bits 1 and 2 high  

		sts	UCSR0C,r16			//Setting parity, clock phase, data order, and stop bit 

		ldi 	r16,0x87		//initialize ADC 

		sts 	ADCSRA,r16		//Enables the ADC and sets the prescalars 

		ldi 	r16,0x40		// bit 7 high 

		sts 	ADMUX,r16		// Set internal voltage reference 

		ldi 	r16,0			// All pins low 

		sts 	ADCSRB,r16		// All pins low 

		ldi 	r16,0xFE		// bits 1-7 high 

		sts 	DIDR0,r16		// Turn off all ADC's except the one being used (pin 0) This saves power 

		ldi 	r16,0xFF		// All Pins high 

		sts 	DIDR1,r16		// Disables all pins, saves power 

		ret						// leave subroutine when finished  

	 

.global LCD_Write_Command 

LCD_Write_Command: //take data and write on lcd

	call		UART_Off			// LCD requires access to the UART dual ports, must disable the UART ports to use the LCD 

	ldi		r16,0xFF			;PD0 - PD7 as outputs 

	out		DDRD,r16			// Enables all pins on this port for use 

	lds		r16,DATA			// Store the DATA into r16 

	out		PORTD,r16			// Store the DATA into port D 

	ldi		r16,4				// Enable only the third pin for port B 

	out		PORTB,r16			// Enable only the third pin for port B 

	call		LCD_Delay			// Delay the LCD to write 

	ldi		r16,0				// Disable Port B 

	out		PORTB,r16			// Disable Port B 

	call		LCD_Delay			// Delay LCD again 

	call		UART_On			// Enable UART ports when writing is 							// finished 

	ret						// leave subroutine when finished  

  

LCD_Delay: 

	ldi	r16,0xFA				// store r16 with a very large number 

D0:	ldi	r17,0xFF				// store r17 with an even larger number = 

D1:	dec	r17					// decrement r17 

	brne	D1					// Continue decrementing r17 until it is equal 							 // to r16 

	dec	r16					// Now decrement r16 

	brne	D0					// Repeat this process again until equal 

	ret						// Creates a small loop that takes a while to 							// complete 

  

.global LCD_Write_Data 

LCD_Write_Data: //write data for lcd

	call	UART_Off				// USART must be turned off to use the LCD 

	ldi	r16,0xFF				// All pins high 

	out	DDRD,r16				// Enable all data registers for D 

	lds	r16,DATA				// store data into r16 

	out	PORTD,r16				// store data into PORT D 

	ldi	r16,6					// set pins 1 and 2 high 

	out	PORTB,r16				// Enable pins 1 and 2 for PORT B 

	call	LCD_Delay				// Delay LCD for writing 

	ldi	r16,0					// Disable PORT B 

	out	PORTB,r16				// Disable PORT B 

	call	LCD_Delay				// Delay LCD again 

	call	UART_On				// re-enable UART after finished with LCD 

	ret						// leave subroutine when finished 

  

.global LCD_Read_Data 

LCD_Read_Data: 

	call		UART_Off			// Disable UART when using LCD 

	ldi		r16,0x00			// ALL data registers disabled for D 

	out		DDRD,r16			// ALL data registers disabled for D 

	out		PORTB,4			// Enable pin 3 for B 

	in		r16,PORTD			// Read PORTD input and write into r16 

	sts		DATA,r16			// store this value into DATA 

	out		PORTB,0			// DISABLE port B 

	call		UART_On			// Re-enable USART when finished 

	ret						// leave subroutine when finished 

  

.global UART_On 

UART_On: 

	ldi	r16,2					//initializes port D 

	out	DDRD,r16				//initializes port D 

	ldi	r16,24					// Enable bits 2 and 5 

	sts	UCSR0B,r16				// Enables character size and data registry 							// interrupt 

	ret						// leave subroutine when finished 

  

.global UART_Off 

UART_Off: 

	ldi	r16,0					// Turn all pins off 

	sts 	UCSR0B,r16				// Sets USART pins off 

	ret						// leave subroutine when finished 

  

.global UART_Clear 

UART_Clear: 

		lds	r16,UCSR0A			// read UCSR0A pins and save to r16 

		sbrs	r16,RXC0			// Checks if parity, doubled transmission 							// speed, and parity error are enabled 

		ret					// Leave subroutine if not 

		lds	r16,UDR0			// store any USART data into r16 

		rjmp	UART_Clear			// Loop to finalize clearing 

  

.global UART_Get 

UART_Get: 

		lds	r16,UCSR0A			// read UCSR0A pins and save to r16 

		sbrs	r16,RXC0			// Checks if parity, doubled transmission speed, and parity error are enabled 

		rjmp	UART_Get			// If not error and loop 

		lds	r16,UDR0			// store any USART data into r16 (the information you are getting) 

		sts	ASCII,r16			// store this data into ASCII variable 

		ret					// leave subroutine when finished 

  

.global UART_Put 

UART_Put: 

		lds	r17,UCSR0A			// read all pins active in UCSR0A and save them to r17 

		sbrs	r17,UDRE0			// checks if the USART port is set to communication mode (for reading and wiritng) and there is no parity error 

		rjmp	UART_Put			// If not error and loop 

		lds	r16,ASCII			// Loads the most recent ASCII string to r16 

		sts	UDR0,r16			// Stored this value to Data Register 0 for displaying 

		ret					// leave subroutine when finished 

  

.global ADC_Get 

ADC_Get: 

		ldi	r16,0xC7			// pins 0,1,2,6,and 7 high 

		sts	ADCSRA,r16			// Loads the ADC Prescalar, enables the	ADC, then starts the first conversion 

A2V1:		lds	r16,ADCSRA			// Loads the ADCSRA pins to r16 

		sbrc	r16,ADSC			// Checks if prescalar division factor is 8 

		rjmp 	A2V1				// If not error and loop  

		lds	r16,ADCL			// Loads ADC low bit data into r16 

		sts	LADC,r16			// Stores this value into LADC 

		lds	r16,ADCH			// Loads ADC high bit data into r16 

		sts	HADC,r16			// Stores this value into HADC 

		ret					// leave subroutine when finished 

  

.global EEPROM_Write 

EEPROM_Write:       

		sbic    EECR,EEPE 

		rjmp    EEPROM_Write		; Wait for completion of previous write 

		ldi	r18,0x00			; Set up address (r18:r17) in address register 

		ldi	r17,0x05  

		ldi	r16,'F'				; Set up data in r16     

		out     EEARH, r18      ;address high 

		out     EEARL, r17		;address low       

		out     EEDR,r16			; Write data (r16) to Data Register   

		sbi     EECR,EEMPE			; Write logical one to EEMPE 

		sbi     EECR,EEPE			; Start eeprom write by setting EEPE 

		ret  

  

.global EEPROM_Read 

EEPROM_Read:					     

		sbic    EECR,EEPE     

		rjmp    EEPROM_Read		; Wait for completion of previous write 

		ldi		r18,0x00		; Set up address (r18:r17) in EEPROM address register 

		ldi		r17,0x05 

		ldi		r16,0x00    

		out     EEARH, r18    

		out     EEARL, r17		    

		sbi     EECR,EERE		; Start eeprom read by writing EERE 

		in      r16,EEDR		; Read data from Data Register 

		sts		ASCII,r16   

		ret 

  

  

		.end 