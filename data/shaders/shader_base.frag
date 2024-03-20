#version 330
in vec3 Normals;
in vec3 FragPos;
out vec4 outColor;


void main ()
{
	vec3 norm = normalize((Normals));

	vec3 lightPos = vec3(1000.0, 1000.0, 10000.0); 
	vec3 lightColor = vec3(1.0, 1.0, 1.0);
	vec3 ambient = vec3(0.3, 0.3, 0.3);
	vec3 objectColor = vec3(0.3, 0.3, 0.3);



	vec3 lightDir = normalize(lightPos - FragPos);

	float diff = max(dot(norm, lightDir), 0.0);
	vec3 diffuse = diff * lightColor;
	vec3 result = (ambient + diffuse) * objectColor;
	outColor =  vec4(result, 1.0f);
}