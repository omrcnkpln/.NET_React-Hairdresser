

export const convertBase64 = (file:File) =>new Promise((resolve , reject )=>{
        const fileReader = new FileReader();
        fileReader.readAsDataURL(file);

        fileReader.onload = () =>{
            let fileInfo = {
                name: file.name,
                type: file.type,
                size: Math.round(file.size / 1000) + ' kB',
                base64: fileReader.result,
                file: file,
              };
            
            resolve(fileInfo)
        }

        fileReader.onerror=(error) =>{
            reject(error)
        }

    })