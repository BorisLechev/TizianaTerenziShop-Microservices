pipeline {
  agent any
  stages {
    stage('Verify Branch') {
      steps {
        echo "$GIT_BRANCH"
      }
    }
	stage('Run Unit Tests') {
	  when {
		expression { fileExists('src/TizianaTerenzi.sln') }
	  }
	  steps {
		dir('src') {
		  powershell 'dotnet test TizianaTerenzi.sln'
		}
	  }
	}
	stage('Check Docker') {
	  steps {
		powershell 'docker --version'
		powershell 'docker compose version'
	  }
	}
	stage('Docker Build') {
	  steps {
        dir('src') {
		  powershell 'docker compose --profile build-only build base-runtime'
          powershell 'docker compose build'
          powershell 'docker images'
        }
      }
	}
	stage('Run Application') {
      steps {
	    dir('src') {
		  powershell(script: 'docker compose up -d')
		}
      }
    }
    stage('Run Integration Tests') {
      steps {
        powershell './Tests/ContainerTests.ps1'
      }
    }
    stage('Stop Application') {
	  steps {
	    dir('src') {
		  powershell(script: 'docker compose down')
		  powershell 'docker volume prune --force'
		}
      }
      post {
	    success {
	      echo "Build successfull! You should deploy! :)"
	    }
	    failure {
	      echo "Build failed! You should receive an e-mail! :("
	    }
      }
    }
	stage('Push Docker Images on DockerHub') {
	  steps {
		script {
	      docker.withRegistry('https://index.docker.io/v1/', 'DockerHub') {
			def tag = "1.0.${env.BUILD_ID}"
			
            def baseImage = docker.image("borislechev/base-runtime:8.0")
			baseImage.tag(tag)
            baseImage.push(tag)
            baseImage.push('latest')
			
			def identityImage = docker.image("borislechev/tizianaterenzimicroservices-identity-service")
			identityImage.tag(tag)
            identityImage.push(tag)
            identityImage.push('latest')
			
			def productsImage = docker.image("borislechev/tizianaterenzimicroservices-products-service")
			productsImage.tag(tag)
            productsImage.push(tag)
            productsImage.push('latest')
			
			def cartsImage = docker.image("borislechev/tizianaterenzimicroservices-carts-service")
			cartsImage.tag(tag)
            cartsImage.push(tag)
            cartsImage.push('latest')
			
			def notificationsImage = docker.image("borislechev/tizianaterenzimicroservices-notifications-service")
			notificationsImage.tag(tag)
            notificationsImage.push(tag)
            notificationsImage.push('latest')
			
			def ordersImage = docker.image("borislechev/tizianaterenzimicroservices-orders-service")
			ordersImage.tag(tag)
            ordersImage.push(tag)
            ordersImage.push('latest')
			
			def identityGatewayImage = docker.image("borislechev/tizianaterenzimicroservices-identitygateway-service")
			identityGatewayImage.tag(tag)
            identityGatewayImage.push(tag)
            identityGatewayImage.push('latest')
			
			def cartsGatewayImage = docker.image("borislechev/tizianaterenzimicroservices-cartsgateway-service")
			cartsGatewayImage.tag(tag)
            cartsGatewayImage.push(tag)
            cartsGatewayImage.push('latest')
			
			def administrationImage = docker.image("borislechev/tizianaterenzimicroservices-administration-service")
			administrationImage.tag(tag)
            administrationImage.push(tag)
            administrationImage.push('latest')
			
			def watchdogImage = docker.image("borislechev/tizianaterenzimicroservices-watchdog-service")
			watchdogImage.tag(tag)
            watchdogImage.push(tag)
            watchdogImage.push('latest')
			
			def webClientImage = docker.image("borislechev/tizianaterenzimicroservices-webclient-service")
			webClientImage.tag(tag)
            webClientImage.push(tag)
            webClientImage.push('latest')
		  }
		}
	  }
	}
  }
}
