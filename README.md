The general architecture of the developed FuzzyKIDE application consists of the following components:
1. Database (Input)
    Stores the initial data as Comma Separated Values (CSV).
    Each input value consists of its name by which it is presented in the knowledge base, an explicit value, and a confidence factor between 0 and 1 that reflects the user's confidence in the explicit value.
2. Knowledge base
    Consists of inference rules and linguistic variables used for reasoning by the inference core.
3. The inference core
    Deals with the processing of input data based on the knowledge base and is implemented using the SNePS semantic network.
4. Graphical User Interface (GUI)
    Allows the user to interact with the knowledge base, execute the fuzzy inference core, view the fuzzy and crisp inference results.

The appliaction users have an option to work with knowledge base, change inference settings, upload input data, perform fuzzy inference and investigate inference results.
Overall interaction process could be described as follows:
![image](https://user-images.githubusercontent.com/27803595/186419841-82a31962-a390-4eb7-acfb-41d1d44d04a9.png)

The GUI of the application consists of 4 main windows.
1. Initial window is login\registration form. Its purpose is to separate application users to work with individual knowledge bases in order to simplify user experience, so that users with different roles are not clustered with information not required for their reasoning processes.
After successful login\register users can gain access to other 3 main windows.
![image](https://user-images.githubusercontent.com/27803595/186420031-23e16206-c09d-4ba1-b361-957614364ed5.png)
2. First next windows is a window to change inference settings, like lower boundary of confidence factor. This setting allows inference core to establish the lower boundary of certainty at which application recommends taking actions on suggested decisions.
![image](https://user-images.githubusercontent.com/27803595/186420209-db6020bb-a195-4839-b565-1cdc1d971e0a.png)
3. Second window is purposed to work with multiple knowledge bases by adding\modifying profiles, which consist of implications rules and linguistic variables sets.
The application GUI allows adding of inference profiles by adding name and description to them. After profile is created application allows adding of rules and variables sets to be achieved in two ways:  by adding them individually or by uploading them from files in batches.
Once adding of inference rules and variables is done, users have ability to filter though existing rules and variables and also to modify them in the same window.
![image](https://user-images.githubusercontent.com/27803595/186420442-9fd36dd8-27a3-4fbf-bb38-cd1f39c9e27f.png)
![image](https://user-images.githubusercontent.com/27803595/186420457-cac7bebc-422c-45ad-b2d3-c7e288c4c028.png)
![image](https://user-images.githubusercontent.com/27803595/186420467-c6acb278-d402-4da2-9f7e-b4226d302a12.png)
![image](https://user-images.githubusercontent.com/27803595/186420490-9d4fb7e8-0fc4-4ec4-84f3-d5058c77c860.png)
![image](https://user-images.githubusercontent.com/27803595/186420506-20c5514e-b265-48d5-b972-ff3cefedca63.png)
4. Third windows is purposed to perform inference itself, once user decides that knowledge base is ready to be used for inference. In that windows user selects which profile (knowledge base) to use, uploads initial data (CSV) and performs inference.
Once inference is finished the same windows is used to display and filter inference results. Application decision suggestion could also be seen on the same screen, when specific part of inference result is selected.
![image](https://user-images.githubusercontent.com/27803595/186420570-626bcc63-42a4-4473-8f53-90319fe3939b.png)

