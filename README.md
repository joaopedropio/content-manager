#### Future plans for this service ####

All the services of the Social Network Projects were designed with micro services in mind. I'm gonna change the mindset with this one.
This project has 4 sections:
 * User interface: will handle the user input (files, metadata, configurations, etc)
 * Download file: this part will get the file and save to the appropriate folder
 * Convert file: check if the file needs convertion, convert and segment it
 * Upload file: this section uploads the file to the user or import it to the Content Importer Service
 
The goal is to get this section and separate them. Each one will become a service. This actions will increase high availability, scalability and more....