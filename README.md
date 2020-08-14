# Assets Monitor

This project was developed as a test for a job interview and therefor is not production ready.
It's pourpouse is to recover information about a asset (e.g. PETR4) from brasilian market and send an email if it is above MAX_VALUE, or below MIN_VALUE parameters.

## Configuration files

To send the email the program expects you to configure your SMTP service on file email.config.json

## Running the Program

To run the program you may build it using visual studio or cmd and call the .exe file passing the following parameters:

file.exe ASSET_NAME MAX_VALUE MIN_VALUE

Every 10s the program will try to recover info about the asset and will send the email if necessary.
Beware that the asset info recovery api we are using is limited to 400 calls a day.

## Language

All output messages are in PT-BR

