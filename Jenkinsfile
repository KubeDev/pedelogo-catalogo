/* primeira pipeline */

pipeline{
    
    /*
    environment {
      imagename = "harshmanvar/node-web-app"
      registryCredential = 'docker'
      dockerImage = ''
    }
    */
    
    agent any

    stages{
        
        stage('Get Source'){
            steps{
                git url: 'https://github.com/jheffersonn/pedelogo-catalogo.git', branch: 'main'

                /*
                git([url: 'https://github.com/harsh4870/node-js-aws-cloudbuild-basic-ci-cd.git', branch: 'main', credentialsId: 'github'])
                */
            }
        }

        stage('Docker Build'){

            steps{
                script{
                    dockerapp = docker.build("jeffersondevops/pedelogo-catalogo:${env.BUILD_ID}",
                        '-f ./src/PedeLogo.Catalogo.Api/Dockerfile .')
                }
                /*
                script {
                    dockerImage = docker.build imagename
                }
                */
            }
        }


        stage('Docker Push Image'){
            
            steps{
                script{
                    docker.withRegistry('https://registry.hub.docker.com', 'dockerhub' ){
                        dockerapp.push('latest')
                        dockerapp.push("${env.BUILD_ID}")

                    }

                /*
                script {
                    docker.withRegistry( '', registryCredential ) {
                        dockerImage.push("$BUILD_NUMBER")
                        dockerImage.push('latest')

                    }

                    */
                }
            }
        }

        stage('Deploy Kubernetes'){
            
            steps{
                
                sh "cat ./deployment.yaml"
                sh "kubectl --kubeconfig=kubeconfig get pods"
                sh "kubectl --kubeconfig=kubeconfig apply -f ./deployment.yaml"
              
              /*  withKubeConfig([credentialsId: 'kubernetes']){
                    sh 'kubectl apply -f ./deployment.yaml'
                
                }*/
            }
        }
    
    }
}