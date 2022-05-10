// react-router-dom components
import { Link } from "react-router-dom";

// prop-types is a library for typechecking of props.
import PropTypes from "prop-types";

// @mui material components
import Container from "@mui/material/Container";
import Grid from "@mui/material/Grid";
import { Box, Typography } from "@mui/material";

function DefaultFooter({ content }:any) {
  const { brand, socials, menus, copyright } = content;

  return (
    <Box>
      <Container maxWidth={false} sx={{ maxWidth:'90%'}}>
        <Grid container sx={{ justifyContent: 'space-between' }} >
          <Grid item xs={12} md={3} sx={{ mb: 3 }}>
            <Box>
              <Link to={brand.route}>
                <Box component="img" src={brand.image} alt={brand.name} maxWidth="2rem" mb={2} />
              </Link>
              <Typography variant="h6">{brand.name}</Typography>
            </Box>
            <Box display="flex" alignItems="center" mt={3}>
              {socials.map(({ icon, link }:any, key:any) => (
                <Typography key={key}
                >
                  {icon}
                </Typography>
              ))}
            </Box>
          </Grid>
          {menus.map(({ name: title, items }:any) => (
            <Grid key={title} item xs={6} md={2} sx={{ mb: 3 }}>
              <Typography
                
                mb={1}
              >
                {title}
              </Typography>
              <Box component="ul" p={0} m={0} sx={{ listStyle: "none" }}>
                {items.map(({ name, route, href }:any) => (
                  <Box key={name} component="li" p={0} m={0} lineHeight={1.25}>
                    {href ? (
                      <Typography
                      >
                        {name}
                      </Typography>
                    ) : (
                      <Typography
                        
                      >
                        {name}
                      </Typography>
                    )}
                  </Box>
                ))}
              </Box>
            </Grid>
          ))}
          <Grid item xs={12} sx={{ textAlign: "center", my: 3 }}>
            {copyright}
          </Grid>
        </Grid>
      </Container>
    </Box>
  );
}

// Typechecking props for the DefaultFooter
DefaultFooter.propTypes = {
  content: PropTypes.objectOf(PropTypes.oneOfType([PropTypes.object, PropTypes.array])).isRequired,
};

export default DefaultFooter;
