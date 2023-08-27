# Welcome to MiniJira App!
## Created from scratch, took 23 hours to build it. Including this documentation

Simple .Net Core 6 App tasks manager  **application** meant to be sent to **Ramsoft DotNet interview** for **Thiago Vendrame de Bona**

Used technologies:
 - Fully **RESTfull asyncronous** back-end
 - Dotnet core 6 - **Minimalism code style**
 - MongoDB
 - JWT Authenticator
 - WebSocket - **For live notifications**
  - Fully mocked unit tests that cover all the cases
  
 Used tools:
  - Visual studio 2022 community edition
  - Docker desktop for windows
  - MongoDBCompass
  
Including ->   Simple console application to see the MiniJira tasks and board at **live**

# MiniJira 1.0 Features

 - Fully functional SwaggerUI with Bearer Auth available
 - Manage **multiple** boards
 - Create and authenticate new user
 - Manage multiple columns inside the boards
 - Can create/update/delete multiple tasks in different boards
   - Can move tasks between columns, ex: **Todo, In progress, In test, done**
   - Can create custom columns
   - Can favorite tasks, favorited tasks are in the top of the board tasks
   - Can assign a task to a user that you've created
   - Can include multiple attachments in the tasks
 - Live connection with the back-end
 - Authenticated real time console application to see the board

# Project Files Structure
![Image](https://lh3.googleusercontent.com/pw/AIL4fc--hJM4nGlhQQb64lyyYw7o85DkiSC5sVWcLs8mRnozolWCYIHY29kCnFRKGCq8joahlIDH6i-j90TqZZxSNmbjx-H9t8zgiWmBlAZwnBwjD0y2IJI-u2vzcWdgsc-SdB1DrP3KBy3G4SRqLU-XkCxS7vmzkEl8G5VursMp8PYS4u90n2cEWPndf3leltu_T5s2bv9fs4LS8PKxR_qMu-6Wm1fYhHNthqNamGc7eG1jd6dxbesxBdTct5K7bQ0V18_xHGnuh39ZoNVnBATs8DGwzXX1te0rOr7iYy3jd7edaUP6_iDIRg2JSmEuchKk_r-h1U6DkliXONRvDG09xCiqKMNPBUfdSCA98gIo87KVtBKAN9XUV0u0C4-0HIJz1iYlisELAhOiq-InIMgWzsJUiAvDotQXmIt5zHp5tM1tLdyEbBKGeG6v9H4FimPfYhl9UbpK7h9PhK81V1HxdrQchUlMisHde-8DBne7AXOBczn4Dxd7QajkAd-6JOEvpXdjTHQVg3OZ2ZI1eO5bsgshJJGW-EDorzqRtgc4NGuUxkYNWcYtjDPXOnrPlc5BA8sACy83IYyRzSyWyBmRVnblilpiM1vQtMNsERgY5EAnmQP9TVn4ck9sSYYA1rRaQhSB8KZS306c6PBur8bWATFgfbn1ysVIvo3hlaD_np5N4iIlWcuGPOtfEQA1w3u8xPMyfSmIYlpbIR8Jn536EFIjt0e7bmfC8Z0By_SfzoTllmC6352as-_vgvHYLK6IunfVjE6B4pecM97YJ49CMzLyj-mx0oFgzZ60ghrsVTjM1-EEKnxQ47mXW4RvND8Kcmq1yt4pp0fHUxiP9DAvyADMLbU1WO1UNkG0oNeoSfnzaR4ZdSScS5eCKpbNjCjE-hNK5muh9k4vVTQ7RUHXW1Y=w328-h378-no?authuser=0)

- The project is organized in four main projects, splitted by DDD
    > Dotnet.MiniJira.API
    > Dotnet.MiniJira.Domain 
    > Dotnet.MiniJira.Application
    > Dotnet.MiniJira.Infrastructure
- The project contains 7 folder.
	> Between folder 1. and 4. we find all the required project to run the **MiniJira** sub-menu.
	
	> **Docker files** folder has the dockerfile to instance a new MongoDB in your local docker container
	
    > **Tests** folder contains all mocked tests to keep integrity in the source code
- Secondary projects
-   > Dotnet.MiniJira.Tests
-   > Dotnet.MiniJira.Notification.Client - Connect to the **back-end WebSocker** app for **live notifications**

## How to run locally the project?

You'll need a mongoDb docker container running or a mongodb local instance
To start a new mongodb docker instace simple run the the following command
> docker compose -f "Mongo-docker-compose.yml" up -d --build 

Then 
 > Configure your VisualStudio to run multiple projects at once
 > **Dotnet.MiniJira.API** and **Dotnet.MiniJira.Notification.Client**

![Image](https://lh3.googleusercontent.com/pw/AIL4fc9uojB7_z-80xaJuaLIvOuo-Gt2YdjjaXQ2XBhjmMEKRQt-3tBKIpBFyudMCXOGvpjIxi2EYkNDLBCm1ZNSsrpn8pJzsoprm1PDxWEAxyGf8X_xA7S77yGv_CZUYD9A9Z2dkvEazwo1fFISawscypxjZJT6VN8l5wNMR76eV_r5934mgNfNyR57pni-YzlcrdfsUYZJuEh31WUzb5S5zstBR4ubDOYNGd4KdCC4Ofl8Qa_SGzH5IArPhmD2v361OKACUNYi6_IvvtaFD4oTPtbMHUK42PyNwA2BGCqH5CcavX0riC51bz6zEwUa2rWXrBVT63tJ3lHl4XWzSdTvkXnP6BDskqSHqNm8UxQwj0HaXYm-LCFyzs6v_o_S3AxkyPNkLMHJ9GccBGGp__4Pj4jDcsSUK1ldJiYEuGShmAduGBu0s4nK8CqOrNobrcph6YyHHBKo2yNe9UmJDL3Y07ztysCnLDfngWTTX9dcYRlB46UrOO1KMbnPLLZlq-tsGtmY2eyepWiABRsgf9duCIWMqihr7nMQAdo2YXe74kvK37TJDQBy1tuR2bAWFKzjozxCvUgSZn6zpd0bmG0GarllBLB7ZYCEjtCVWahSGVJAOgeLnbvL7qK4673thBm3KlFodPZEwON_AinWvU10JxX4fWq05HdP697yYovJs_YU7Bb7vo5QFFypui_Fay5nOm7YvuAENe6-QcmnZYXL_u0XQmJfhc6cZb7aeMBGoubjOT0B5SKrvLzObMQmH79LQ5oel9EA5ZOuVFgBG8nfXn5sRt4ooEf48oKWYj_ZAGoF1iAY4ohk6UtqnzfA6XFIMJLZyOpCp9Nr0g1LVEpOupC6lKW3fB4NKSyL1yeYzvZwZNApR0AMcNS0-e6V49KBdl7rC_cJPUpcQSUAN_vgKfU=w1149-h578-s-no?authuser=0)
 
 When the Dotnet.MiniJira.API runs, it automatically seeds the data base with some information. It'll automatically create a new board called **Ramsoft - Issues board**, will create four column **Todo, In progress, Testing and done**
 
 You will have three users available to use
 
 > Administrator -> Username[admin] Password[admin]
 > Developer      -> Username[developer] Password[developer]
 > Tester -> Username[tester] Password[tester]
 >
We will see two consoles, one is the **WebAPI** and the other is the **client**, the client is used like a front-end app to see what's going on in the **MiniJira**, the client receive live notification via **webSocket**

![Image](https://lh3.googleusercontent.com/pw/AIL4fc_yHsh6vT_iFnKAaKuW4BUFjhkwc00URVHS6pFnnwsaQ_TPXoZjs4xpPJNWd0BGuOHQhfP8iT8ao2vIGhxBjx-LpjgCVrQfCHqv2vWfUyqQCku_Zh7r57EnC8zq6nvjWapWXFAqWMaMi4lhwAEsKmkZAgWBrMncHfA_RrxM3Lm-k67kOlHyFG1K4NZTrx32AXFZJpbnfqTrZIBbXUSEePigmXgtYG7s8G8JeS8SWPeDYxwahzcxKg7u3H4fCLF-PyJHMTKD7QDKdWJnwHsbauYVQbS5Jjjg7PwKlPMb1aEyYqVFNC6SsPKZ8BEFuBPjt9n-eCMohmXOAsymE_4BXb-9bwofmPqHTARx95-ndbd0xFFeEQDyRafsIhXvw7CGt-Msq3jMvBdkhI6HyKw5PQ8x3OPIucN-DVfvZt2RtZSc1dcQA1_Kt4GO1UknB5IjuT-wbOyikL70F0i4cPq_UECH4JnAiIvgxHqO7zH26GlCpYTgk9bGkxvHmu5Mav8QweF9NydWGAEhedo-OasaipSOkZKlHWNL96J-z549PL2me4dNdejwG9rrxArE6dSO_fwLRD5ssgmCNMbCDecoKPrUZRGV3zLDjaW1sqioz0AFJ-KxSrlQMR2wzRl-nmTIQ7r8eW_D7UlahYM5c8Mm6M8b1CnWaAoJS5ngNaVHXPIYYVVZgQgJ9bVZxJ5Y2dux7rJ4XqaPlDX7xGIKGDjrHCU_C4Qhd1s5jbwz8hcmYl2Bfi6e7vBRZy9Sif9Gj_4CuwF5M2BOS9czxvLZKSqvQG9ztfIeRHfDU-zKrjNvEJ3VntkFXzVPRGGRfLar4ylYeDTRSwuE0ZsUh_hkmkBztF2jmCsN24qzOVswx4MCmR5u-2g0Nagh0hRvVF8eHnF2-5qJpVlDA7VErHVeXoMJwJU=w989-h595-s-no?authuser=0)


## Fully functional Swagger UI with Bearer Auth
> Swagger URL is http://localhost:7100/index.html
![Image](https://lh3.googleusercontent.com/pw/AIL4fc-NKi3I0T3JReHGdhufJ2Qt4EXIeOaid-LvLv3Hc2somR6BM7JV99YVz1n5ZLKw92-8tsHyztN-vd1GbLRMSmYxlmT8sOCeIaObtXbLk29ffazljOzdvCCYEbaja6aixZ4rEaEPohxoTz6_M522WqKlU6aaan8BYmYoVXXDoj3MoTskxwY5WS9pgBy_9e90hr6UXun6RSSRualu60hf2ZNU49yqz8_GBvQY8QJ1dacHBiVLzwlQd41SSgc6UUdYq2oQAxgb1nV3g1yz8HrbbPv3EdsBF-8X_B-JWngrIm3tCaLmvH8l1IuwnxdZEr9iOUvDjK1exG0X3U2EmmcyRtYR7GYjrlQXf0uWRE6rMxmhfoWqaBH70JDKWDuh7bAa0h5ZcEiPzm_gAjIDoAJVET0Z7sxN_3hmPHc2wLR6LwqE2ztYnKP6WyueIEwNU0PMhUl6JUhd8niIxG2lFBv9sNgrjXxjR5wm0pI3PH5aNwTt7YwrN4szpkVehI5k6zA93L0_nxojwTtfW3x5XfGty6UoYc1kS9SNZiH8a7Ns360pxm_YVCJiJNL-HFWDJcU92QBJiD0iVo2sVLvnGXWvDgzhZBio1Ll7QFZrZIjYhN0L8ju-YCaKEQYSxFCc8mNn33awew8eZnJ4mHvDMvScINQzBWVUhL7apUoA47OdjgkNqqZDRj7UYfyhTDPYJ3Wlm7_7BlMDaSIMZ2pTU0UcCHdnVWk--dFzABwLyEk4YoTp2Wx7bD-Ws77HMUfhZlFZfPYaKAwPlXFj3Fbs7G0KyWb0HynoSZl51GHsVpduLeH-hT_6bAm4Fg7TiwsXhcriJl9kzykpxkxukeJ6OuS3xzjyj3NHezLUioNznfDAWKc_x67l49uDESOpvRPiu_bV4OuRtkJur61ICY6KoJOMFng=w608-h447-s-no?authuser=0)

The first thing to do is to authenticate yourself with the existing admin, develop or test user, or create your own user.
> Scroll down until the bottom of Swagger UI, to the section **Users** and **authenticate** with username **admin** and password **admin**
 ![Image](https://lh3.googleusercontent.com/pw/AIL4fc_UFRrnmfDvjACQ0SoPRgZJtrN_F1KrzIJRWRNgH1R5VnV-E-RIaNhzfNVAcaS3dzhObLMjibIs7KguYRajbhm7g43-BG979jvHQ2w_CWjj-OLgRJjBJ4ZLSD16o4Ps3l2SX1-WtgUsmJZwqF-HvidjjdKnp8i9yu-2OgK19NFH006LwyGO476N-eM4NcLYJnjVlXb62AYJW2S5Yh8HYmq_8HynVgdPr1TesjKHSYlg0EEs3YVhPJ3ypCHs3FTV0YCpYQQLMu-j0wo7pKEfA9Hpj3iwJq4eWt6H6nCaQx_J1GYgkvz1FJMJiIYVH5J4oyXAbLdcfzlvb-6zlJvA7kwYjS_GktzWf2XyIJaV0jVpyB0ib-2gfqvskmCa2fcau29S9y2p7dF4U6BdF6A6Gxt2_gdP3C7LY6YQOFfdqXUbYtgtoJ85oGmlzF6dGP8nGebplulbjWM6yU80YVeH57qYzksYO8CYi_yc3FzKRjTUbtdF9maHFZ7MWCSstFgMwSf6kWvsUkwFYImWjOTxjkPL0YkL0PaZ3ysS1PlLLD8avsr45Cl6uxSoRhWDPvTBs9ed_Vv_8QZ2TRBZz4f2os9SZIZIEknumuUJWb27QJoTm_uZd1neKP-DcgX4wpPiR9njYWgZV9x_8q8YQEzPsnCcD4e0EkwpqTjAxxC1HpHV38fcWtOMlcksyzHagRaiVHDfXJvsjmq4oeLk6IxStwd4zqDS4eLpfseglS3BzD2wlZFkCviJbd9fm3jHF1oDrFqIAG5b1OSy6phFQJw0w6wGNhr5GBztIfN2CKHKKREpcF9uKcuSCzKES5WS0Q-oTOYS6fk2pP2HQ9xYBwYLVjuMl0uVFfnSoatyEl9lExkb3DgY6sgFE3kfUMscJQ5QIVaEXisP9HgWX_fqovqkJlM=w568-h630-s-no?authuser=0)


Take the JWT token in the response and apply it to the swagger by scrolling to the up and click on button "Authorize", and it will automatically notify the client with the existing boards in the data.

![Image](https://lh3.googleusercontent.com/pw/AIL4fc_yj9kMY8D1HrPfkdCKTccEggLMpRCMbuK7IilMkDIoIEjXidGBPN3Qqx3CZvXSdvAUo0L9fAQLcTqFKB-JJMbTnps1t1AT_76EXJ5JWc3uvca7bSGRTjddi2YNTqmn2GBvE_h5Lk51qsfpcba7HFHpM1cohSB9Dqq9cBMmtDmh4PPPZQsoW4oDAZpqKUXNc4gOxntRxpsz8uJvih3wio1-N-RLDHq-TNSfFvnWO46fJneZfdwEdKEgggYhcvgiiX30HyjGWj1i2K0x1FLpmm-hN53RyQ1aUYGBLS23kKWP0p8A-aoeVW_94ZfEhjWzb6HVPFwEBTzpaMY9_5AAbN-K17z_RDmc4Hql9vK91kNhtm1AR75hohO97DFeO-e2JZxlbOuhNwJRwtVTa7NHbkQrAlAIdr7_4y6STN9sCXCcBvB8hWqOSESi4oxZ1KPnK1dDVULqUMRsW_5LYIwhmVNAOuYP0AaDtfepjzRazXDQJe_o_rxo0up7jxvyWy_lfpck9yFXCpBq9AfW0unSK25pUCSSzX-UI4MhdemyF9G0U8zpg5oVkP52PhalbBf5SAFVAbbIPRYoRS9-ri69FiIYjsyHGJIzKlqdSDJuzxHvxHgPB-R2MjWfq34DM7daKRPtXA3xiP1rdNG6y9z6rZbY0PxTdR0RpNftJaxIzNefNIAI61gpG1YLAChSRFqy6vHZOUVmhyLTMWy4GJsHIPa2a0KNr768QbdPekbTpI0--feOvSvK6YbYz5_n8spKTfCNugMa8o63F8q0Zi1gv-t5pQPfsQIW5xUVASfQFypZZ7djuC4BlBr6YL7ENmsDt2HW22MQhLvMcdgilcejZQ1NbhbiB3DFNYiGpr1ICpcr1jfivrPdhDAr2QVYh1Wkg7-TEtS8cBj9-QTCz8fViKY=w612-h172-s-no?authuser=0)

![Image](https://lh3.googleusercontent.com/pw/AIL4fc_597F9nlOhNRlBXWSaDRPEeHtnc57iMIJdOWyzajdWKfbRNcFj4kQX-Ed15Vvjb7UFcuYg6V5I4DP-r9BUnRcZ6WDZ2e4FNKquSRw3Qlyv_vGLN4AJHB6JUFA5jU4dvLmZDdluPp7jdY4smA4oGtp1dO7JG2xhuvrBGw3ujkdS_OZOVNf5LzYpicRqVO54EJNkYSnv-rUO6I1OSjvvVHWrhIiIMUkEPREJy95F9dxIHjJUd_Ki3LbOfmNRxaaUciZhIilTMZKqTN0KRtmghyWMdiviL6vUqSfQNzFlQSat49-PTL3Zucko3Q5fD0Vx95l_p37nr_l6oLNMYXDErJKR-hgTCXB2fvQgKHN8HdeW03qcEn9agckLzHRL_StostSH6QxflP8QLm4RIeNz0UdV8usw93Y-mca0vdR5bj_HGe_pP4kOuZhh0xGoPMCv3FFJcfwXluwiPD_m9Y4YO-oKIG12UfKGoHGTLO62NYDfA_Px8bciGiqOpOQx_URKKnh78zGRSxjEPXX_3TOLPW6e-C3KJTCZiZXWgd0_gFJM3KGlN7hbsgnknXDm_HTUjy-H7N-RRdk-q4HlHkJyGfJeLGpn8YwTFL3SAx-1FP5ExI4bxjd_2MLuZfun-y2TUME_LEbmbQnLkk8ffw95mMij_0S5K7GOBJKE4fOyTZjaVmteeBiBZmNBJOcvOfW2SEIr7fyX028Gjk_-cx9ncqj4w8NGe0hl62rfreAlyP2TdcAyfT_LZggQQqQlQIiRbEioWhJeAKSt7EjV2n2RLcXbCiY-ppipaRhwO-ys13u0zUqMCcDw1B9TTbvErQzkvr4KMZjTV5P7Zz45UFTCvchWJg90eAgh6Kem2Q_MV7rT86QQ_bUSh0QuCTucMKkckJNIb0adDQrSJFRMfLHmS7Y=w936-h317-s-no?authuser=0)

And Now you are ready to use the API through Swagger UI

## Connecting the Client to the Notification server via WebSocket

Now you need to inform in the client the username and password and press enter, then it'll connect to the WebSocket and will start receiving live updates from the server.
![Image](https://lh3.googleusercontent.com/pw/AIL4fc87vR1mfojk-GFhWY5bOq82YOlS1L-4ixF1VezXGvqI0lz4_LQ78hfS-k8_4aaKd0S9KlKTVy7wn6bTGurxI8ZFfT9qK5onB6b7Av_AQotN8UOsBHEpct6NAVd4TI0EA_mbJHs_oY6BYn04WaPJkdBNrsRH9xbQ23Xtpfh2dMYbb4artwqkqB-RqNDEcYwDLwOlmj_sk52v9Rynbqlox9XCV1beKNMi53v0KKQZfAXjDdsWa3G7ddxPAGxFqeSys-8esnfNsBuxmYUX3R1AK-rxNueklN_o0vEN21L7iXq4W6Q68bzEIOgkvmbR_lX9zV0fTp5ebh4FBAhnCenbhV8lGuzqcTKMtHsq7EOh0DHmzNujmw9a2pGGmi3wr_AEut39KddVjoNtdthPOAPG3HUY8pcWUbQWfqm5NISKP3I8BaDhvozMNWev7AdzG9xF15gMC4z4_v_r09t92aCpwpPA-WrYQfQHCUrQcpAEr5waCcMlWqCy7fDJBmG8qDw_zkc7XVy_IzhosM6B2PiYQrs5xlO4G3LN_Sj0rR0IR6_hru3uiR7TF9myF6dtVvqK6S1Sp7_byPnACz-Uh2ImlznuNjzWguAqC7w2XyyEGJ9jZ7ml_ittxc-73WZi7z-Z6nA9ZtEWJePlsFNwRYFUH2dDIQdgUP6QEiiKz0Rc16rJL3uK99KGKrf7W8_furzXPTXSihtI9CWCl9xlcD8pXSDY-TvOfyGpajtuY3eDN5awn-B99wTnqw-7u72wf6Gotdl4807PFzUy1_JQFbmyrlhFyKzdPpsi3VyUq2e2vqO8lnue5uGg3OalE00gD5btkAXRPALcK3zoo5yJGGhEnA3AFXQug-6c9z8VJh_6Mtt9W_qX2Ahedz3qb82_ifQLVgFdH69HVlDSp17msFiNjUM=w975-h317-s-no?authuser=0)

**The live notification are received whenever one of the following thing happen**
> User is authenticated
> Create/Update/Delete a **board**
> Create/Delete a column in any **board**
> Create/Update/Delete **task**
> Add attachments to a **task**
> Assign a user to **task**
> Move a **task** between different column

## Notifications examples

Now in your SwaggerUI in the section "Users", when you try to authenticate, if the authentication goes well you will receive in your client app the current status of the MiniJira
![User](./assets/k11k12.PNG)

![User](./assets/k9k10.PNG)

Now let's create a new board

![User](./assets/k7k8.PNG)

Now we can see in the client that a new board was created, with the default column and without tasks

![User](./assets/k5k6.PNG)

Now let's create insert a task on it, using the boardId and ColumnId that we received in the result of the Board's creation

![User](./assets/k3k4.PNG)

Now we can see in the client that a new task was inserted in the board in the column Todo

![User](./assets/k1k2.PNG)



## Business Rules

**User module**

|    route|action|admin|develop|tester
|----------------|-----------|----------|-------|-------
|/Authenticate|POST|true|true| true
|/Create|POST|true|true| true|
|/{id}|GET|true|true| true|

**Board module**

|    route|action|admin|develop|tester
|----------------|-----------|----------|-------|-------
|/Board|POST|true|false| false
|/Board|PUT|true|false| false|
|/Board|GET|true|true| true|
|/Board/{id}|DELETE|true|false| false|
|/Board/{id}|GET|true|true| true|
|/Board/DeleteColumn|DELETE|true|true| true|
|/Board/CreateColumn|GET|true|true| true|

**User module**

|    route|action|admin|develop|tester
|----------------|-----------|----------|-------|-------
|/Board|POST|true|false| false
|/Board|PUT|true|false| false|
|/Board|GET|true|true| true|
|/Board/{id}|DELETE|true|false| false|
|/Board/{id}|GET|true|true| true|
|/Board/DeleteColumn|DELETE|true|true| true|
|/Board/CreateColumn|GET|true|true| true|

![image info](./assets/1a.PNG)
> **/Users/Create** -> Do not create **admin** profiles
> It just creates the **Develop** or **Tester** profiles
> The only way  of having an admin user is the back-end that seed the db in the app's initialization


![User business rules](https://lh3.googleusercontent.com/pw/AIL4fc8MNUCIwox9XX47idUbXJyfcoAyrFNVcBSz2z1RRy10TNan94MRMd9QA7PdIiyJXQu1H_5JrLfpnIZ8rz_enQx5uUHZ6-nmduJay3H11X3gYC9CgsJ2bM9OjFMgLDWrsVcVoZbhu8fWL4J_CIa-aGsAmMWbvjmsgO2B9thMF3UdoB_O13PYWjZSWQwH3ObE708jEaNcKvcCKo-tNtC__aOMlbGPsCkde1dwEwBtFOtScNFhlrmYP_oAhsm0Z2gIaf1qLalt3lXkNVRpREu_S-HdA5vN5-TD4QVfJ54vhbSTZmuC95L0CY7Rh1Lw_9WA9xtYTQ1E9oCqzY_rqZFfE7XRE6NPO7tDjQbPhWg65tGPLgeypfztrDqVEIn6Uv59BuM76qhK5KFjeclBBfHrZYqqUeRK9OscJoXFdeM1BCAayU57aBZ0odw_ny4-uHbqJIdWnw1B-yHiypTKC7-v_hQEyH4KNVGyvAeXZpedBKlpyPrkrKV3IZUGWT59xPjkjKzRaL4U93KmadU7thFGZIMtUEZANyPzD7zGMDTboQKBolvCuEI3SBVhmShHXKrZ6EC7o5-7oVsWq5O_i2bE3SiGxQKIb9Ne-pa1DS4zHe--mz9nsBw39pcl-WrkmJV7LD_hj3YygYl_RsbG2ehMNczRmluFc-BCTZVWS5HXKIRHtf_NfDwqNGAzii03r69Odt8lgZA0VxwIF0y8hSX0Th0XAAryofnXhZaI3i-Y40T-N2sqQxm5u1x-tRDxBallzkDK-qwyc3q-t8gUvS3c1jnOdQXhoxSf91JmvK4GDe1x32GTAvgg-v4ZgheJPxLfdgw8xU4l5qGJsfmmK3GyFYRzykfiiGM5FLua0_wA5pmG7EfYMpSwuEeL-8MtzraehVOsLieIDd0nyuN02QpbN2k=w460-h435-s-no?authuser=0)
> Only **admin** can **CREATE/DELETE/UPDATE** boards information
> **Developer and Testers** can only **CREATE/DELETE** columns in a board

![User business rules](https://lh3.googleusercontent.com/pw/AIL4fc_hmt1cmoyn0Ez4yH5iwI3rPGP6wcG-rcuRoctLUPFYRwm4qi1C4mdBOkKReK6yIlMbXO0MzW8J1MRxCJLhIk29NwUSIyg50pF-itnoSchdqiJv2c3ZkrjwKQ5ugGwCcllISVZcEjnju1wR4G-NLMJg_5q8oezb-LlMR7r8glFgZHdV89S-zQCFolMMtHZzbdzsAZbglhBHspz2w5PObvsUcLvla3R-3JEQfMwLSr1v4sjBQX5_lXmTlhAbR6M9OMbkxwtH_OH9ZycmvsIWoTdhLhqa5FOB-PO8NOGUah0nSCbNNJJA5qNJYV_XeSVAE8WREo0dA0UmCcm3390s8LT2p8SK0ISoQ-BvOO2raJMiGJUIwb8Uye4A-tsCHh5U0zGdqc_lP_kTSmL9cRQorLeof9K28qfPHIwCte87rt1_nzr9pTV_2YQe7orvi6YFJt0n4gkkObKeLDD2k0iCovVLhOfV4RofKU_tiOpB675e4752XKo1DAPiPowwM_Jl6KTFJSf0pmVcOwvvyu40BTvCOwtcis-I2i9H1ESeScCnDhOLF1s4V2oMAwz-vP_g7tXpyui_9UxPHVuqInGsj42C4E1yhjxaBdVlI9zQaFPnH6u2Rplu8I5L86NZ2F32Lgw6DzWOf-aurXKCYMBHnGQ424mMvgl6UR-Va_noXQI1z0MWqUnJKsa8i2T-rAEZFgGT5g0rWG6l81awt-75C_q0c12W9JyR4jElDdpY5dSwe74Rwkj6d6AASC3XwJKbB1_fkw7i_jT81uj_80SoaMmI8NbIwQ_oO4AKgKy-wyX7mVkBylwPvk6aSfeG35asP2pYndO8N1hJvJqdVg9D6VLklynZg5npmuZSrh753zybUw5zaZc6kOj_I7qWAvVegnqzIEYvhV7tmVKdTP2p5nQ=w527-h505-s-no?authuser=0)
> All the profiles can apply any action on any tasks

# Unit tests
  Fully mocked unit tests that cover all the cases can be found in the folder tests. 33 unit test.
![enter image description here](https://lh3.googleusercontent.com/pw/AIL4fc84wF4uBvipD8Q4MSKgWKzGO_MKjx3yuoBsypFk8npvwK16f-sNMFefSb5nn_GrUJFazgq2wTQxQAq_4YxxhKJjEujqt7lEVe2bBTXja4ShgibvoVK-yYRhpqf-pwdtqanBodk518Fhk0HIUbg0pYig8CJJHXAJ0FwlW32wTaXS2gRZL5Ic-Oc5-uNKdR3p5DRMyqUhuYXtFdUOsLHmu8r_TV4T9igphmHiMyOyM5L-YgvwqofhEVWjgEhD_CTT8GtRVd2AYfwSFPUBn5SRfqgVXFHJpor8CCi0lwuV-RTLjUmOjiWtOnP2jxPEc4RAzIYAY6LhK_vmmURR3QzsgFGUKNEnXHxArNHT_88Hf9ye3338g3fyVIHO-MmPvzw4UROJFF09-Cc8rty_tZUtDCIvFg7NybfvJ_GkkZp4vHyz_Dcy3nZkngk-HiInNz-5mlMSQwbhTQTlw2DPQuyJwnhLyB7oeszHRbi2PwaQI-SGJ8J2P4c1LZwtBufHIqsE1W112ugNfLYNrMQyKrSrKosTrC9N9V9OMkbcGZWgfhZn-swEJg9MO2Ijnnfoh7BbjietsQS8THcAS7kZnbhW0RJPpAW7JEdTxA_RZwud-IY_-CJ4cd4A3TrlvDgaWofq8qDHfLijgN6RMewXZ1TH5PMNhU46rlKJ_FzRAbwehlqNEDLbYvd37K599cOFecj3BT4QUb5NJsraGEsrtMFeC9hPI1gRRscowtT18wsOGCoKC8T_TIvSYuRjfECbPkS_wlI76kR9gwxFAdr1sNaXSFiNs0GeLVsaItpXOHVGb0jo0sm4tJttVeW2_Wsx4EMfJsQaLNqhcYcdX4r8UKOUAK-J-pDrjiTshAkLqZeIsyh_KBC6kRwobA9j5EpFgo1erReYA0Y_y_qH-6VFx5HCtVY=w584-h190-no?authuser=0)